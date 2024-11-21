using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class MiniGridController : MonoBehaviour
{
    private Image[,] gridCells;
    [SerializeField] private Sprite defaultSprite; 
    [SerializeField] private Color defaultColor = Color.clear;

    private void Start()
    {
        gridCells = new Image[3, 4];
        int index = 0;

        foreach (Transform child in transform) 
        {
            
            int row = index / 4;
            int col = index % 4;
            
            Image cellImage = child.GetComponent<Image>();
            if (cellImage != null)
            {
                gridCells[row, col] = cellImage;

                // Set default sprite and color
                cellImage.sprite = defaultSprite;
                cellImage.color = defaultColor;
            }
            index++;
        }
    }


    public void LoadHouseFromLot(LotController lot, GameObject minigrid)
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

            // Load the house data into the grid
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    TileData tileData = lot.houseData.tiles[row, col];

                    if (tileData.sprite.name != "BuildingTileEmpty")
                    {

                        gridCells[row, col].sprite = tileData.sprite;
                        gridCells[row, col].color = tileData.color;
                        gridCells[row, col].SetNativeSize();

                    }

                    //Debug.Log($"Loading house state from lot: {lot.gameObject.name} tile at ({row}, {col}): sprite={tileData.sprite}");


                }
            }
        
    }
}
