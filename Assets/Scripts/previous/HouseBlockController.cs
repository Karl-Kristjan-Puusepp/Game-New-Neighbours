using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HouseBlockController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject draggedCopy;
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public bool isDoor;
    public int windows = 0;
    private CanvasGroup canvasGroup;
    [HideInInspector]
    public bool wasDropped = false;  // New flag to track successful drops

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        wasDropped = false;  // Reset drop flag
        
        // Create a copy of this object
        draggedCopy = Instantiate(gameObject, canvas.transform);
        draggedCopy.name = "DraggedCopy";

        // Add reference to this controller to the copy
        HouseBlockController copyController = draggedCopy.GetComponent<HouseBlockController>();
        if (copyController != null)
        {
            copyController.enabled = false;  // Disable the script on the copy
        }

        // Get the RectTransform of the copy
        RectTransform draggedRectTransform = draggedCopy.GetComponent<RectTransform>();
        
        // Set the initial position to match the pointer
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out position);
        draggedRectTransform.anchoredPosition = position;

        // Setup the dragged copy
        CanvasGroup draggedCanvasGroup = draggedCopy.GetComponent<CanvasGroup>();
        draggedCanvasGroup.blocksRaycasts = false;
        draggedCanvasGroup.alpha = 0.7f;

        Debug.Log("Started dragging - Copy created");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedCopy != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out position);
            draggedCopy.GetComponent<RectTransform>().anchoredPosition = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;

        // Only destroy the copy if it wasn't successfully dropped
        if (draggedCopy != null && !wasDropped)
        {
            Debug.Log("Drag ended without successful drop - destroying copy");
            Destroy(draggedCopy);
        }
    }

    public GameObject GetDraggedCopy()
    {
        return draggedCopy;
    }
}

