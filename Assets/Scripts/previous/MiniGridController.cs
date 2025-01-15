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

    private Button parentButton;
    private LotController lotController;

    private void Awake() {
        gridCells = new Image[4, 3];
        int index = 0;
        parentButton = GetComponentInParent<Button>();
        if (parentButton != null)
        {
            lotController = parentButton.GetComponent<LotController>(); // Get the LotController component
        }

        foreach (Transform child in transform) 
        {
            
            int row = index / 3;
            int col = index % 3;
            
            Image cellImage = child.GetComponent<Image>();
            if (cellImage != null)
            {
                gridCells[row, col] = cellImage;
                //Debug.Log($"Initialised r:{row}, c:{col} in minigrid");
                // Set default sprite and color
                cellImage.sprite = defaultSprite;
                cellImage.color = defaultColor;
            }
            index++;
        }

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (gridCells[row, col] == null)
                {
                    Debug.LogError($"Grid cell at [{row}, {col}] is null. Check your GameObject hierarchy!");
                }
            }
        }
    }


    public void DisplayHouse(House house)
    {
        Awake();
        if (house == null) return;
        bool hasNonDefaultTiles = false;
        // Load the house data into the grid
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                HouseTile tile = house.tiles[row, col];
                if (tile == null) continue;
                if (tile.sprite == null) continue;
                if (tile.color == null) continue;
                if (tile.sprite.name != "BuildingTileEmpty")
                {       
                    if (gridCells[row, col] == null)
                    {
                        Debug.LogError($"Grid cell at [{row}, {col}] is null!");
                        continue;
                    } 
                    gridCells[row, col].sprite = tile.sprite;
                    gridCells[row, col].color = tile.color;
                    gridCells[row, col].SetNativeSize();
                    hasNonDefaultTiles = true;
                    Debug.Log($"Grid cell r:{row}, c:{col} set to {tile.sprite.name}");
                }
            }
        }

        if (parentButton != null && hasNonDefaultTiles)
        {
            parentButton.interactable = false;
            lotController.hasHouse = true;
            Debug.Log("Parent button has been set to non-interactable.");
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
