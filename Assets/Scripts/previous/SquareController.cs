using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class SquareController : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public event Action OnTileChanged;
    public Image squareImage;
    private Button squareButton;
    private const string FILLED_TAG = "FilledTile";  // Define the tag as a constant
    private bool IsDoor;

    [SerializeField]
    public GameObject buildingTilePrefab;
    
    private Sprite defaultSprite;
    private Color defaultColor;
    //public int doorInt;
    private void Awake()
    {
        squareImage = GetComponent<Image>();
        squareButton = GetComponent<Button>();
        
        // Store the initial values
        if (squareImage != null)
        {
            defaultSprite = squareImage.sprite;
            defaultColor = squareImage.color;
        }
        
        // If we don't have a building tile prefab reference, try to find it
        if (buildingTilePrefab == null)
        {
            buildingTilePrefab = Resources.Load<GameObject>("BuildingTile");
        }

        // Make sure the "FilledTile" tag exists
        // You'll need to manually add this tag in Unity's Tags & Layers settings
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop triggered");
        
        GameObject droppedObject = eventData.pointerDrag;
        Debug.Log($"Dropped object: {(droppedObject ? droppedObject.name : "null")}");

        if (squareButton != null && !squareButton.interactable)
        {
            Debug.Log("Square is not interactable. Drop ignored.");
            return; // Return if the square is not interactable
        }

        HouseBlockController originalController = droppedObject.GetComponent<HouseBlockController>();
        
        if (originalController != null)
        {
            droppedObject = originalController.GetDraggedCopy();
        }

        if (droppedObject != null)
        {
            if (originalController.isDoor)
            {
                IsDoor = true;
            }
            else
            {
                IsDoor = false;
            }
            Image draggedImage = droppedObject.GetComponent<Image>();
            Debug.Log($"Dragged image component: {draggedImage != null}");
            
            if (draggedImage != null && squareImage != null)
            {
                // Store the sprite before destroying the object
                Sprite newSprite = draggedImage.sprite;
                Color newColor = draggedImage.color;
                
                // Update the square's sprite and color
                squareImage.sprite = newSprite;
                squareImage.color = newColor;

                
                // If this square has a button component, update its colors too
                if (squareButton != null)
                {
                    
                    ColorBlock colors = squareButton.colors;
                    Color newNormalColor = colors.normalColor;
                    newNormalColor.a = 1f;
                    colors.normalColor = newNormalColor;
                    squareButton.colors = colors;
                }
                

                // Mark as filled
                gameObject.tag = FILLED_TAG;
                
                Debug.Log($"Square updated - New sprite: {squareImage.sprite != null}");

                // Mark the drop as successful
                if (originalController != null)
                {
                    originalController.wasDropped = true;
                }

                // Trigger the change event
                OnTileChanged?.Invoke();

                if (newSprite.name == "door" || newSprite.name == "Roof right" || newSprite.name == "RoofRight" || newSprite.name == "RoofCenter")
                {
                    var gridController = FindObjectOfType<HouseBuildingGridController>();
                    gridController.SetSquaresBackToInteractable();

                }

                // Destroy the copy after a short delay
                Destroy(droppedObject, 0.1f);

                

                if (newSprite.name != "Roof right" && newSprite.name != "RoofRight" && newSprite.name != "RoofCenter") // Replace with the actual sprite name
                {
                    MakeSquareAboveInteractable();
                }

                
            }
        }
    }
    private void MakeSquareAboveInteractable()
    {
        // Find this square's position in the grid
        for (int row = 0; row < HouseBuildingGridController.Instance.gridSquares.GetLength(0); row++)
        {
            for (int col = 0; col < HouseBuildingGridController.Instance.gridSquares.GetLength(1); col++)
            {
                if (HouseBuildingGridController.Instance.gridSquares[row, col] == this)
                {
                    int aboveRow = row - 1;
                    if (aboveRow >= 0) // Check if there's a row above
                    {
                        SquareController squareAbove = HouseBuildingGridController.Instance.gridSquares[aboveRow, col];
                        if (squareAbove != null)
                        {
                            squareAbove.SetInteractable(true); // Make it interactable
                            Debug.Log($"Square above at row {aboveRow}, col {col} made interactable.");
                        }
                    }
                    return; // Exit once the square is found
                }
            }
        }

        Debug.LogWarning("Square's position in the grid could not be found.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ResetToDefaultTile();
    }

    public void SetTile(HouseTile tile)
{
    if (tile == null)
    {
        Debug.LogWarning("SetTile received a null tile. Resetting to default.");
        ResetToDefaultTile();
        return;
    }

    if (squareImage != null)
    {
        if(tile.sprite == null) Debug.Log("Tile sprite was null");
        if(tile.color == null) Debug.Log("Tile color was null");
        squareImage.sprite = tile.sprite ?? defaultSprite; // Use default if sprite is null
        squareImage.color = tile.color != default ? tile.color : defaultColor; // Use default if color is null

        if (squareButton != null)
        {
            ColorBlock colors = squareButton.colors;
            colors.normalColor = squareImage.color;
            squareButton.colors = colors;
        }

        gameObject.tag = (tile.sprite != defaultSprite) ? FILLED_TAG : "Untagged";
        OnTileChanged?.Invoke();
    }
}


    private void ResetToDefaultTile()
    {
        if (buildingTilePrefab != null)
        {
            // Get the Image component from the prefab
            Image prefabImage = buildingTilePrefab.GetComponent<Image>();
            if (prefabImage != null && squareImage != null)
            {
                // Reset to the prefab's sprite and color
                squareImage.sprite = prefabImage.sprite;
                squareImage.color = prefabImage.color;

                // Reset button colors if applicable
                if (squareButton != null)
                {
                    ColorBlock colors = squareButton.colors;
                    colors.normalColor = prefabImage.color;
                    squareButton.colors = colors;
                }

                // Reset the tag
                gameObject.tag = "Untagged";
                
                Debug.Log("Square reset to default BuildingTile");
                
                // Trigger the change event
                //OnTileChanged?.Invoke();
            }
        }
        else
        {
            // Fallback to stored default values
            if (squareImage != null)
            {
                squareImage.sprite = defaultSprite;
                squareImage.color = defaultColor;

                if (squareButton != null)
                {
                    ColorBlock colors = squareButton.colors;
                    colors.normalColor = defaultColor;
                    squareButton.colors = colors;
                }

                gameObject.tag = "Untagged";
                Debug.Log("Square reset to stored default values");
                
                // Trigger the change event
                //OnTileChanged?.Invoke();
            }
        }
    }

    public bool IsFilled()
    {
        return gameObject.CompareTag(FILLED_TAG);
    }

    public HouseTile GetCurrentTile()
    {      
        return new HouseTile(squareImage.sprite, squareImage.color);
    }

    public void SetInteractable(bool isInteractable)
    {
        GetComponent<Button>().interactable = isInteractable;
    }
}