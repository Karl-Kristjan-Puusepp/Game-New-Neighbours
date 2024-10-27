using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


public class LotController : MonoBehaviour
{
    private Button lotButton;
    public HouseData houseData;  // Current house state
    [SerializeField] private GameObject defaultTilePrefab;

    private static HouseBuildingGridController houseBuildingGrid;
    private static LotController currentSelectedLot;

    private void Awake()
    {
        lotButton = GetComponent<Button>();
        lotButton.onClick.AddListener(OnLotClicked);
        
        if (houseBuildingGrid == null)
        {
            houseBuildingGrid = FindObjectOfType<HouseBuildingGridController>();
        }

        CreateDefaultHouse();
    
        // Initialize default house if none exists
        if (houseData == null)
        {
            CreateDefaultHouse();
        }
    }

    private void CreateDefaultHouse()
    {
        houseData = new HouseData();
        
        // Initialize the tiles array
        houseData.tiles = new TileData[3, 4];

        Image defaultTileImage = defaultTilePrefab.GetComponent<Image>();

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                houseData.tiles[row, col] = new TileData(
                    defaultTileImage.sprite,  // Set to the default sprite
                    defaultTileImage.color,   // Set to the default color
                    false                     // Default to unfilled
                );
            }
        }
        
        Debug.Log("Default house created.");
    }


    public void OnLotClicked()
    {
        // If we're clicking on a different lot than the current one
        if (currentSelectedLot != this)
        {
            // Save the current grid state to the previous lot if there was one
            if (currentSelectedLot != null)
            {
                houseBuildingGrid.SaveCurrentStateToLot(currentSelectedLot);
            }

            // Update the current selected lot
            currentSelectedLot = this;
            
            // Load this lot's house data into the grid
            houseBuildingGrid.LoadHouseFromLot(this);
            
            Debug.Log($"Switched to lot: {gameObject.name}");
        }
    }
}
