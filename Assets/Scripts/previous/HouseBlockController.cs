using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HouseBlockController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject draggedCopy;           // The dragged copy of this GameObject
    private Canvas canvas;                    // The Canvas in which we place the dragged copy
    private RectTransform rectTransform;      // The RectTransform of this (the original) GameObject
    private CanvasGroup canvasGroup;          // The CanvasGroup for this (the original) GameObject

    private Vector2 originalPosition;         // Store the original position in case we need it
    private Vector2 localClickOffset;         // Distance between object's pivot and mouse
    [HideInInspector]
    public bool wasDropped = false;           // Flag to indicate if a successful drop occurred

    public bool isDoor;
    public int windows = 0;

    private void Awake()
    {
        // Find the main (root) Canvas in the scene.
        canvas = FindObjectOfType<Canvas>();

        // Get necessary components on THIS object.
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        wasDropped = false;

        // Instantiate the dragged copy as a child of the canvas.
        draggedCopy = Instantiate(gameObject, canvas.transform);
        HouseBlockController copyController = draggedCopy.GetComponent<HouseBlockController>();

        // Disable the copy's drag logic so it doesn't interfere.
        if (copyController != null) copyController.enabled = false;

        // Keep anchor/pivot/size consistent.
        RectTransform draggedRectTransform = draggedCopy.GetComponent<RectTransform>();
        draggedRectTransform.sizeDelta = rectTransform.sizeDelta;
        draggedRectTransform.anchorMin = rectTransform.anchorMin;
        draggedRectTransform.anchorMax = rectTransform.anchorMax;
        draggedRectTransform.pivot = rectTransform.pivot;

        // Configure the copy's CanvasGroup.
        var draggedCanvasGroup = draggedCopy.GetComponent<CanvasGroup>();
        if (draggedCanvasGroup == null)
            draggedCanvasGroup = draggedCopy.AddComponent<CanvasGroup>();

        draggedCanvasGroup.blocksRaycasts = false;
        draggedCanvasGroup.alpha = 0.7f;

        // Ensure the Image on the copy inherits the original color.
        Image originalImage = GetComponent<Image>();
        Image copyImage = draggedCopy.GetComponent<Image>();
        if (copyImage != null && originalImage != null)
        {
            copyImage.color = originalImage.color;
        }

        var copySpriteName = draggedCopy.GetComponent<Image>().sprite.name;

        if (copySpriteName == "door")
        {
            var gridController = FindObjectOfType<HouseBuildingGridController>();
            gridController.SetBottomRowInteractable();
        }
        if (copySpriteName == "Roof right" || copySpriteName == "RoofRight" || copySpriteName == "RoofCenter")
        {
            var gridController = FindObjectOfType<HouseBuildingGridController>();
            gridController.SetTopRowInteractable();
        }

        // Set the dragged copy position to the mouse position.
        UpdateDragPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedCopy == null) return;

        // Update the dragged copy position to follow the mouse.
        UpdateDragPosition(eventData);
    }

    private void UpdateDragPosition(PointerEventData eventData)
    {
        // Use the world position to directly position the dragged copy under the mouse.
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector3 worldPos
        );
        draggedCopy.transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset the original block's CanvasGroup.
        canvasGroup.alpha = 1f;

        if (draggedCopy != null)
        {
            if (!wasDropped)
            {
                var gridController = FindObjectOfType<HouseBuildingGridController>();
                // If the copy was not successfully dropped, destroy it.
                Debug.Log("[OnEndDrag] Drag ended without successful drop - destroying copy");
                Destroy(draggedCopy);
                gridController.SetSquaresBackToInteractable();
            }
            else
            {
                // Apply the color of the dragged copy to the dropped target.
                Image copyImage = draggedCopy.GetComponent<Image>();
                Image originalImage = GetComponent<Image>();
                if (copyImage != null && originalImage != null)
                {
                    originalImage.color = copyImage.color;
                }
            }
        }

        // Clear the reference to the dragged copy.
        draggedCopy = null;
    }

    public void MarkAsDropped()
    {
        wasDropped = true;
    }

    public GameObject GetDraggedCopy()
    {
        return draggedCopy;
    }
}
