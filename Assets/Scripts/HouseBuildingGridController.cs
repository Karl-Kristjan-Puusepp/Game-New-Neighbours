using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class HouseBuildingGridController : MonoBehaviour
{
    private SquareController[,] gridSquares = new SquareController[3, 4];
    
    private void Awake()
    {
        InitializeGrid();
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
            int row = i / 4;
            int col = i % 4;
            gridSquares[row, col] = squares[i];
            
            // Subscribe to each square's changes
            int capturedRow = row;
            int capturedCol = col;
            squares[i].OnTileChanged += () => OnSquareChanged(capturedRow, capturedCol);
        }
    }

    private void OnSquareChanged(int row, int col)
    {
        // Find the currently active lot
        LotController currentLot = FindCurrentLot();
        if (currentLot != null)
        {
            SaveCurrentStateToLot(currentLot);
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

    public void LoadHouseFromLot(LotController lot)
    {
        // Disable the button of the selected lot to mark it as active
        lot.GetComponent<Button>().interactable = false;

        // Enable all other lot buttons
        foreach (LotController otherLot in FindObjectsOfType<LotController>())
        {
            if (otherLot != lot)
            {
                otherLot.GetComponent<Button>().interactable = true;
            }
        }

        // Load the house data into the grid
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                TileData tileData = lot.houseData.tiles[row, col];
                gridSquares[row, col].SetTile(tileData.sprite, tileData.color);
                /*
                // Update the filled state
                if (tileData.isFilled)
                {
                    gridSquares[row, col].gameObject.tag = "FilledTile";
                }
                else
                {
                    gridSquares[row, col].gameObject.tag = "Untagged";
                }
                */
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
                var (sprite, color) = square.GetCurrentTile();
                
                lot.houseData.tiles[row, col] = new TileData(
                    sprite,
                    color,
                    square.IsFilled()
                );
            }
        }
        Debug.Log($"Saved house state to lot: {lot.gameObject.name}");
    }
}