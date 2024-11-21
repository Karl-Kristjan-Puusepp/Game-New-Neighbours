using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;


public class LotController : MonoBehaviour
{
    private Button lotButton;
    public HouseData houseData;  // Current house state
    public String requirementText = "Build a house!";
    public TextMeshProUGUI Requirement;
    public GameObject requirementPanel;
    public int requiredDoors = 1;
    public Button DoneButton;
    public GameObject minigrid;
    public bool firstHouse = false;

    public TextMeshProUGUI EndGameText;
    public GameObject endPanel;

    [SerializeField] private GameObject defaultTilePrefab;

    private static MiniGridController miniGridController;
    private static HouseBuildingGridController houseBuildingGrid;
    private static LotController currentSelectedLot;

    private void Awake()
    {
        endPanel.SetActive(false);
        lotButton = GetComponent<Button>();
        lotButton.onClick.AddListener(OnLotClicked);



        if (firstHouse)
        {
            lotButton.interactable = true;
            DoneButton.onClick.AddListener(OnDoneClicked);
        }
    
        else
        {
            lotButton.interactable = false;
        }

        if (houseBuildingGrid == null)
        {
            houseBuildingGrid = FindObjectOfType<HouseBuildingGridController>();
            //Debug.Log("housebuilding grid ", houseBuildingGrid);
        }

        if (miniGridController == null)
        {
            miniGridController = FindObjectOfType<MiniGridController>();
        }
        CreateDefaultHouse();
        
        requirementPanel.SetActive(false);

  

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
        

        houseBuildingGrid.gameObject.SetActive(true);
        // If we're clicking on a different lot than the current one
        if (currentSelectedLot != this)
        {
            // Save the current grid state to the previous lot if there was one
            if (currentSelectedLot != null)
            {
                houseBuildingGrid.SaveCurrentStateToLot(currentSelectedLot);
            }
            houseBuildingGrid.gameObject.SetActive(true);
            // Update the current selected lot
            currentSelectedLot = this;
            requirementPanel.SetActive(true);
            Requirement.text = requirementText;
            
            // Load this lot's house data into the grid
            houseBuildingGrid.LoadHouseFromLot(this);
            
            Debug.Log($"Switched to lot: {gameObject.name}");
        }
    }
    public void OnDoneClicked()
    {
        //lotButton.enabled = false
        //HouseCompleted = true;
        houseBuildingGrid.SaveCurrentStateToLot(currentSelectedLot);
        miniGridController.LoadHouseFromLot(this, minigrid);
        houseBuildingGrid.gameObject.SetActive(false);
        endPanel.SetActive(true);

        if (firstHouse) { 
            Debug.Log($"{houseBuildingGrid.doorInt} for lot {lotButton.name}");

            if (houseBuildingGrid.doorInt == requiredDoors)
            {
                EndGameText.text = "Yippee !! You have built a house with 1 door !";

            }
            else
            {
                EndGameText.text = "You did not build a house with 1 door :(";
            }
        }

        DoneButton.gameObject.SetActive(false);

    }
}
