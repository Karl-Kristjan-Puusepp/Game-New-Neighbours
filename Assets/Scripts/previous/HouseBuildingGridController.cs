using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.U2D;
using UnityEngine.Tilemaps;

public class HouseBuildingGridController : MonoBehaviour
{
    private SquareController[,] gridSquares = new SquareController[4, 3];
    public int doorInt=0;
    
    private void Awake()
    {
        InitializeGrid();
        gameObject.SetActive(true);
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
            
            // Subscribe to each square's changes
            int capturedRow = row;
            int capturedCol = col;
            //squares[i].OnTileChanged += () => OnSquareChanged(capturedRow, capturedCol);
        }
        
        LoadActiveHouse();

    }

    public void SaveAsActiveHouse() {
        House currHouse = new House();
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 3; col++)
            {   
                HouseTile currTile = gridSquares[row, col].GetCurrentTile();
                if(currTile == null){
                    Debug.Log($"Tile null at row:{row} and col:{col}");
                    continue;
                } 
                currHouse.tiles[row, col] = currTile;                       
            }
        }
        ActiveHouse.SetHouse(currHouse);   
    }

    public void LoadActiveHouse() {
        if (ActiveHouse.CurrentHouse == null){
            Debug.Log("Loading active house failed, active house set to null");
            return;
        } 

        for (int row = 0; row < 4; row++) 
        {
            for (int col = 0; col < 3; col++) 
            {
                HouseTile houseTile = ActiveHouse.CurrentHouse.tiles[row, col];
                //if (houseTile == null) continue;
                gridSquares[row, col].SetTile(houseTile);
            }
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