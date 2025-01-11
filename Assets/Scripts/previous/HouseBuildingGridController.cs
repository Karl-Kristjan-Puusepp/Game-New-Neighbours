using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.U2D;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using static Reporter;
using TMPro;

public class HouseBuildingGridController : MonoBehaviour
{
    public SquareController[,] gridSquares = new SquareController[4, 3];
    public int doorInt = 0;
    public int windowInt = 0;
    public Image HappyImage;
    //private bool ready = false; 
    public TextMeshProUGUI HappyText;
    public static HouseBuildingGridController Instance { get; private set; }


    private Dictionary<SquareController, bool> interactableStates = new Dictionary<SquareController, bool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of HouseBuildingGridController found!");
            Destroy(gameObject);
        }

        InitializeGrid();
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        InitializeGrid(); // Ensure the grid is set up
        LoadActiveHouse(); // Then load the active house
    }


    private void InitializeGrid()
    {
        SquareController[] squares = GetComponentsInChildren<SquareController>();
        
        if (squares.Length != 12)  // 3x4 grid
        {
            Debug.LogError("Grid does not contain exactly 12 squares!");
            return;
        }

        // Map squares to 2D array
        for (int i = 0; i < squares.Length; i++)
        {
            int col = i % 3;
            int row = i / 3;
            Debug.Log($"Row: {row}, Col: {col}");
            gridSquares[row, col] = squares[i];

            if (row == 3) 
            {
                gridSquares[row, col].SetInteractable(true); // Make interactable
            }
            else
            {
                gridSquares[row, col].SetInteractable(false); // Disable interaction
            }

            // Subscribe to each square's changes
            int capturedRow = row;
            int capturedCol = col;
            //squares[i].OnTileChanged += () => OnSquareChanged(capturedRow, capturedCol);
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    public void CountThings(House currHouse)
    {
        doorInt = 0;
        windowInt = 0;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                HouseTile currTile = gridSquares[row, col].GetCurrentTile();
                if (currTile == null)
                {
                    Debug.Log($"Tile null at row:{row} and col:{col}");
                    continue;
                }
                if (gridSquares[row, col].squareImage.sprite.name == "door")
                {
                    doorInt += 1;
                }
                if (gridSquares[row, col].squareImage.sprite.name == "onewindow" || gridSquares[row, col].squareImage.sprite.name == "roundwindow")
                {
                    windowInt += 1;
                }
                if (gridSquares[row, col].squareImage.sprite.name == "twowindow")
                {
                    windowInt += 2;
                }
                currHouse.tiles[row, col] = currTile;
            }
        }
        Debug.Log("AMOUNT OF DOORS CALCULATED: " + doorInt);
        Debug.Log("AMOUNT OF WINDOWS CALCULATED: " + windowInt);

    }

    public void SaveAsActiveHouse() {
        House currHouse = new House();

        CountThings(currHouse);

        ActiveHouse.SetHouse(currHouse);
        CheckRequirements(doorInt, windowInt);
    }

    private void CheckRequirements(int doorInt, int windowInt)
    {
        CustomerData customerData = SceneData.CurrentCustomerStatic;
        bool requirementsFulfilled = false;
        Debug.Log("Doors required: "+ customerData.doorsRequired+ ", doors on house: "+ doorInt);
        Debug.Log("Windows required: "+ customerData.windowsRequired+ ", windows on house: "+ windowInt);
        if (customerData.doorsRequired!= -1)
        {
            Debug.Log("Doors required: "+ customerData.doorsRequired);
            if (customerData.doorsRequired == doorInt)
            {
                requirementsFulfilled = true;
            }
            else
            {
                requirementsFulfilled = false;
            }
        }
        if (customerData.windowsRequired != -1)
        {
            Debug.Log("Windows required: "+ customerData.windowsRequired);
            if (customerData.windowsRequired == windowInt)
            {
                requirementsFulfilled = true;
            }
            else
            {
                requirementsFulfilled = false;
            }
        }
        if (requirementsFulfilled)
        {
            HappyText.text = $"{customerData.CustomerName} is happy !!";
            SceneData.happyCustomers += 1;
        }
        else
        {
            HappyText.text = $"{customerData.CustomerName} is not very happy...";
        }
        HappyImage.sprite = SceneData.CurrentCustomerStatic.CustomerSprite;
        SceneData.customersTotal += 1;


    }

    public async void LoadActiveHouse() {
        InitializeGrid();
        await Task.Delay(300);
        if (ActiveHouse.CurrentHouse == null){
            Debug.Log("Loading active house failed, active house set to null");
            return;
        } 
        
        Debug.Log($"Loading house {ActiveHouse.CurrentHouse.ToString()}");


        for (int row = 0; row < 3; row++) 
        {
            for (int col = 0; col < 3; col++) 
            {
                HouseTile houseTile = ActiveHouse.CurrentHouse.tiles[row, col];
                //if (houseTile == null) continue;
                gridSquares[row, col].SetTile(houseTile);
            }
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    public void SetAboveBottomRowInteractable(bool isDoorDragging)
    {
        if (isDoorDragging)
        {
            // Store current states only when disabling
            for (int row = 0; row < gridSquares.GetLength(0) - 1; row++) // Iterate above the bottom row
            {
                for (int col = 0; col < gridSquares.GetLength(1); col++)
                {
                    var square = gridSquares[row, col];
                    if (square != null && !interactableStates.ContainsKey(square))
                    {
                        // Save the current interactable state
                        interactableStates[square] = square.GetComponent<Button>().interactable;

                        // Disable the square
                        square.SetInteractable(false);
                    }
                }
            }
        }
        else
        {
            // Restore original states when re-enabling
            foreach (var kvp in interactableStates)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.SetInteractable(kvp.Value);
                }
            }

            // Clear the dictionary after restoring
            interactableStates.Clear();
        }
    }

    /*
    private void OnSquareChanged(int row, int col)
    {
        // Find the currently active lot
        LotController currentLot = FindCurrentLot();
        if (currentLot != null)
        {
            //SaveCurrentStateToLot(currentLot);
        }

    }
    
    
    private LotController FindCurrentLot()
    {
        foreach (LotController lot in FindObjectsOfType<LotController>())
        {
            if (lot.gameObject.GetComponent<Button>().interactable == false)  // Assuming selected lot has button disabled
            {
                return lot;
            }
        }
        return null;
    }
    */

    /*
    public void LoadHouseFromLot(LotController lot)
    {
        // Disable the button of the selected lot to mark it as active
        lot.GetComponent<Button>().interactable = false;
        
        // Enable all other lot buttons
        foreach (LotController otherLot in FindObjectsOfType<LotController>())
        {
            if (otherLot != lot )
            {
                otherLot.GetComponent<Button>().interactable = true;
            }
        }
        /
        // Load the house data into the grid
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                TileData tileData = lot.houseData.tiles[row, col];
               
                gridSquares[row, col].SetTile(tileData.sprite, tileData.color);

                Debug.Log($"Loading house state from lot: {lot.gameObject.name} tile at ({row}, {col}): sprite={tileData.sprite}");

                // Update the filled state
                if (tileData.isFilled)
                {
                    gridSquares[row, col].gameObject.tag = "FilledTile";
                }
                else
                {
                    gridSquares[row, col].gameObject.tag = "Untagged";
                }
                
            }
        }
        
        
    }

    public void SaveCurrentStateToLot(LotController lot)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                SquareController square = gridSquares[row, col];
                var (sprite, color, door) = square.GetCurrentTile();

                if (door && lot.firstHouse)
                {
                    Debug.Log($"lot: {lot.gameObject.name} tile at ({row}, {col}): sprite={lot.houseData.tiles[row, col].sprite}");
                    doorInt += 1;
                    Debug.Log($"{doorInt}");
                }
                lot.houseData.tiles[row, col] = new TileData(
                    sprite,
                    color,
                    square.IsFilled()
                );
                //Debug.Log($"Saving house state from lot: {lot.gameObject.name} tile at ({row}, {col}): sprite={lot.houseData.tiles[row, col].sprite}");
            }
        }
        //Debug.Log($"Saved house state to lot: {lot.gameObject.name}");
    }
    */
}