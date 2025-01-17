using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
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
    public Sprite empty;
    public static HouseBuildingGridController Instance { get; private set; }

    private Dictionary<string, string> colorDictionary;



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

    private void Start()
    {
        colorDictionary = new Dictionary<string, string>
        {
            { "white", "#F1F1F1" },
            { "red", "#D65454" },
            { "yellow", "#FFD464" },
            { "green", "#34B467" },
            { "blue", "#519BEC" },
            { "pink", "#E784D3" },
            { "black", "#2C252A" }

        };
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
            //gridSquares[row, col].squareImage.sprite = empty;

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

        double requirementsTotal = 0;

        double requirementsSatisfied = 0;

        Debug.Log("Doors required: "+ customerData.doorsRequired+ ", doors on house: "+ doorInt);
        Debug.Log("Windows required: "+ customerData.windowsRequired+ ", windows on house: "+ windowInt);
        if (customerData.doorsRequired!= -1)
        {
            if (customerData.doorsRequired == doorInt)
            {
                requirementsSatisfied++;
            }
            requirementsTotal++;
        }
        if (customerData.windowsRequired != -1)
        {
            if (customerData.windowsRequired == windowInt)
            {
                requirementsSatisfied++;
            }
            requirementsTotal++;
        }
        if (customerData.widthRequired != -1)
        {
            int widthInt = 0;
            for (int col = 0; col < 3; col++) {


                if (gridSquares[3, col].squareImage.sprite.name != "BuildingTileEmpty")
                {
                    widthInt++;
                }
            }
            if (widthInt == customerData.widthRequired) requirementsSatisfied++;

            requirementsTotal++;
        }
        if (customerData.heightRequired != -1)
        {
            bool heightFulfilled = false;
            for (int col = 0; col < 3; col++)
            {


                if (gridSquares[4-customerData.heightRequired, col].squareImage.sprite.name != "BuildingTileEmpty")
                {
                    heightFulfilled = true;
                }
            }
            if (heightFulfilled) requirementsSatisfied++;

            requirementsTotal++;
        }
        if (customerData.roofColorRequired != "")
        {
            requirementsSatisfied += CheckRoofColor(customerData.roofColorRequired);
            requirementsTotal++;
        }
        if (customerData.wallColorRequired != "")
        {
            requirementsSatisfied += CheckWallColor(customerData.wallColorRequired);
            requirementsTotal++;
        }
        if (customerData.locationRequired != "")
        {
            if (customerData.locationRequired == "tree")
            {
                if (ActiveLot.forest) requirementsSatisfied++;
                requirementsTotal++;
            }
            if (customerData.locationRequired == "water")
            {
                if (ActiveLot.water) requirementsSatisfied++;
                requirementsTotal++;
            }
            if (customerData.locationRequired == "center")
            {
                if (ActiveLot.center) requirementsSatisfied++;
                requirementsTotal++;
            }
        }
        if (customerData.separateHouses)
        {
            if (gridSquares[3, 1].squareImage.sprite.name == "BuildingTileEmpty" && gridSquares[3, 0].squareImage.sprite.name != "BuildingTileEmpty" && gridSquares[3, 2].squareImage.sprite.name != "BuildingTileEmpty") requirementsSatisfied++;
            requirementsTotal++;
        }


        double requirementspercentage = requirementsSatisfied / requirementsTotal;
        if (requirementsTotal == 0.0) requirementspercentage = 1.0;

        if (requirementspercentage == 1.0)
        {
            HappyText.text = $"{customerData.CustomerName} is happy !!";
            SceneData.happyCustomers += 1;
            Game.AddHappyCustomer(customerData.CustomerName.ToLower());
        }
        else if (requirementspercentage == 0.0)
        {
            HappyText.text = $"{customerData.CustomerName} is devastated...";
        }
        else
        {
            HappyText.text = $"{customerData.CustomerName} feels a bit disappointed.";
            SceneData.happyCustomers += Math.Round(requirementspercentage, 2);
            if (requirementspercentage >= 0.5) Game.AddHappyCustomer(customerData.CustomerName.ToLower());
        }

        Game.AddCustomer(customerData.CustomerName.ToLower());

        HappyImage.sprite = SceneData.CurrentCustomerStatic.CustomerSprite;
        SceneData.customersTotal += 1;


    }

    private double CheckRoofColor(String customerColor)
    {
        for (int col = 0; col < 3; col++)
        {
            for (int row = 0; row < 4; row++)
            {
                if (gridSquares[row, col].squareImage.sprite.name == "Roof right" || gridSquares[row, col].squareImage.sprite.name == "RoofRight" || gridSquares[row, col].squareImage.sprite.name == "RoofCenter")
                {
                    Color color = gridSquares[row, col].squareImage.color;

                    

                    if (ColorUtility.TryParseHtmlString(colorDictionary[customerColor], out Color hexColor))
                    {
                        
                        double colortotal = color.r + color.g + color.b;
                        double colorhex = hexColor.r + hexColor.g + hexColor.b;
                        double vahe = Math.Abs(colortotal - colorhex);
                        Debug.Log($"{vahe} {colorhex} {colortotal}");
                        // Check if the colors are equal
                        if (vahe < 0.01 && vahe >= 0.0)
                        {
                            return 1.0;
                        }
                    }
                }
            }
        }
        return 0.0;
    }

    private double CheckWallColor(String customerColor)
    {
        for (int col = 0; col < 3; col++)
        {
            for (int row = 0; row < 4; row++)
            {
                String spritename = gridSquares[row, col].squareImage.sprite.name;
                if (spritename != "Roof right" && spritename != "RoofRight" && spritename != "RoofCenter" && spritename != "BuildingTileEmpty")
                {
                    Color color = gridSquares[row, col].squareImage.color;


                    if (ColorUtility.TryParseHtmlString(colorDictionary[customerColor], out Color hexColor))
                    {
                        double colortotal = color.r + color.g + color.b;
                        double colorhex = hexColor.r + hexColor.g + hexColor.b;
                        double vahe = Math.Abs(colortotal - colorhex);
                        Debug.Log($"{vahe} {colorhex} {colortotal}");
                        // Check if the colors are equal
                        if (vahe < 0.01 && vahe >= 0.0)
                        {
                            return 1.0;
                        }
                    }
                }
            }
        }
        return 0.0;
    }

    public void EraseHouse()
    {
        for (int row = 0; row < gridSquares.GetLength(0); row++)
        {
            for (int col = 0; col < gridSquares.GetLength(1); col++)
            {
                SquareController square = gridSquares[row, col];
                if (square != null)
                {
                    // Set the sprite to the empty sprite
                    square.squareImage.sprite = empty;
                    Button squareButton = square.squareButton;
                    if (squareButton != null)
                    {

                        ColorBlock colors = squareButton.colors;
                        Color newNormalColor = colors.normalColor;
                        newNormalColor.a = 0.87f;
                        colors.normalColor = newNormalColor;
                        squareButton.colors = colors;
                    }
                }
            }
        }

        Debug.Log("All grid squares reset to empty.");
        InitializeGrid();
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

    public void SetBottomRowInteractable()
    {
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

    public void SetTopRowInteractable()
    {
        for (int col = 0; col < gridSquares.GetLength(1); col++)
        {
            
            int interactableCount = 0;
            
            for (int row = 0; row < gridSquares.GetLength(0); row++)
            {

                var square = gridSquares[row, col];
                if (square == null) continue;

                var button = square.GetComponent<Button>();
                if (button == null) continue;

                Debug.Log(square.squareImage.sprite.name + " row " + row + " col " + col);
                if (interactableCount < 2 && button.interactable )
                {
                    
                    // Allow top two interactable squares
                    interactableCount++;

                    if (square.squareImage.sprite.name != "BuildingTileEmpty") interactableCount++;
                    continue;
                }
                if (!interactableStates.ContainsKey(square))
                {
                    interactableStates[square] = button.interactable;
                }

                // Disable the button
                button.interactable = false;
                Debug.Log($"Disabled button at row: {row}, col: {col}");

            }
        }
    
       
    }

    public void SetSquaresBackToInteractable()
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