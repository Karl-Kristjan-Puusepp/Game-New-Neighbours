using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class SquareController : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public event Action OnTileChanged;
    private Image squareImage;
    private Button squareButton;
    private const string FILLED_TAG = "FilledTile";  // Define the tag as a constant
    
    [SerializeField]
    private GameObject buildingTilePrefab;
    
    private Sprite defaultSprite;
    private Color defaultColor;
    
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

        HouseBlockController originalController = droppedObject.GetComponent<HouseBlockController>();
        if (originalController != null)
        {
            droppedObject = originalController.GetDraggedCopy();
        }

        if (droppedObject != null)
        {
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
                    colors.normalColor = newColor;
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

                // Destroy the copy after a short delay
                Destroy(droppedObject, 0.1f);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ResetToDefaultTile();
    }

    public void SetTile(Sprite sprite, Color color)
    {
        if (squareImage != null)
        {
            squareImage.sprite = sprite;
            squareImage.color = color;
            
            if (squareButton != null)
            {
                ColorBlock colors = squareButton.colors;
                colors.normalColor = color;
                squareButton.colors = colors;
            }

            // If the sprite is different from the default, mark as filled
            if (sprite != defaultSprite)
            {
                gameObject.tag = FILLED_TAG;
            }
            else
            {
                gameObject.tag = "Untagged";
            }
            
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
                OnTileChanged?.Invoke();
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
                OnTileChanged?.Invoke();
            }
        }
    }

    public bool IsFilled()
    {
        return gameObject.CompareTag(FILLED_TAG);
    }

    public (Sprite sprite, Color color) GetCurrentTile()
    {
        return (squareImage.sprite, squareImage.color);
    }
}