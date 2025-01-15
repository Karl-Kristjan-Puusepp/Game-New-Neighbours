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
        // Make sure it's the exact Canvas that you want to parent the dragged objects.
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

        // 1) Convert mouse point to Canvas space.
        //    This is where the mouse is within the canvas rectangle.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mousePosCanvas
        );

        // 2) Convert this object's pivot position (which is in world space for a UI element) 
        //    to Canvas space for consistent comparisons.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            rectTransform.position,
            eventData.pressEventCamera,
            out Vector2 objectPosCanvas
        );

        // 3) Compute the offset between where we clicked (mouse) and the object's pivot.
        localClickOffset = mousePosCanvas - objectPosCanvas;

        // 4) Instantiate the dragged copy as a child of the same canvas.
        draggedCopy = Instantiate(gameObject, canvas.transform);
        HouseBlockController copyController = draggedCopy.GetComponent<HouseBlockController>();
        
        // Disable the copy's drag logic so it doesn't interfere.
        if (copyController != null) copyController.enabled = false;

        // Keep anchor/pivot/size if you want.
        RectTransform draggedRectTransform = draggedCopy.GetComponent<RectTransform>();
        draggedRectTransform.sizeDelta = rectTransform.sizeDelta;
        draggedRectTransform.anchorMin = rectTransform.anchorMin;
        draggedRectTransform.anchorMax = rectTransform.anchorMax;
        draggedRectTransform.pivot = rectTransform.pivot;

        // 5) Position the copy so the click point is under the mouse
        draggedRectTransform.anchoredPosition = mousePosCanvas - localClickOffset + new Vector2(440,-250);

        // 6) Configure the copy's CanvasGroup so it doesn't block raycasts while dragging
        var draggedCanvasGroup = draggedCopy.GetComponent<CanvasGroup>();
        if (draggedCanvasGroup == null)
            draggedCanvasGroup = draggedCopy.AddComponent<CanvasGroup>();

        // Make the dragged copy slightly transparent and non-blocking.
        draggedCanvasGroup.blocksRaycasts = false;
        draggedCanvasGroup.alpha = 0.7f;

        // Ensure the Image on the copy is visible
        Image copyImage = draggedCopy.GetComponent<Image>();
        if (copyImage != null)
        {
            copyImage.enabled = true;
            copyImage.color = Color.white;  // Full color (apart from alpha in CanvasGroup)
        }

        // For debugging.
        Debug.Log($"[OnBeginDrag] Created copy: {draggedCopy}, " +
                  $"alpha: {draggedCanvasGroup.alpha}, " +
                  $"Pos: {draggedRectTransform.anchoredPosition}, " +
                  $"Size: {draggedRectTransform.sizeDelta}");

        // Example: special handling for certain sprites
        var copySpriteName = copyImage != null ? copyImage.sprite.name : "NoSprite";
        if (copySpriteName == "door")
        {
            var gridController = FindObjectOfType<HouseBuildingGridController>();
            gridController?.SetBottomRowInteractable();
        }
        else if (copySpriteName == "Roof right" || copySpriteName == "RoofRight" || copySpriteName == "RoofCenter")
        {
            var gridController = FindObjectOfType<HouseBuildingGridController>();
            gridController?.SetTopRowInteractable();
        }

        // Show the original as normal or partially transparent while dragging
        // (Currently set to full alpha, but you can change if you like.)
        canvasGroup.alpha = 1f;  
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedCopy == null) return;

        RectTransform draggedRectTransform = draggedCopy.GetComponent<RectTransform>();

        // Convert mouse position to canvas space again on each frame.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mousePosCanvas
        );
        
        // Keep the original click offset so the block stays exactly under the cursor
        draggedRectTransform.anchoredPosition = mousePosCanvas - localClickOffset + new Vector2(440, -250);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset the original block's CanvasGroup to be fully visible
        canvasGroup.alpha = 1f;

        // If the copy was not successfully dropped on a target, destroy it
        if (draggedCopy != null && !wasDropped)
        {
            var gridController = FindObjectOfType<HouseBuildingGridController>();
            if (gridController != null)
            {
                gridController.SetSquaresBackToInteractable();
            }

            Debug.Log("[OnEndDrag] Drag ended without successful drop - destroying copy");
            Destroy(draggedCopy);
        }

        // Clear the reference
        draggedCopy = null;
    }

    /// <summary>
    /// Called by a drop target to mark this object as successfully dropped
    /// </summary>
    public void MarkAsDropped()
    {
        wasDropped = true;
    }

    /// <summary>
    /// For external checks (if needed)
    /// </summary>
    public GameObject GetDraggedCopy()
    {
        return draggedCopy;
    }
}
