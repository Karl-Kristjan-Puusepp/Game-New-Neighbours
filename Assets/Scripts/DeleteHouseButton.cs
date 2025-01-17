using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class DeleteHouseButton : MonoBehaviour
{
    public HouseBuildingGridController grid;
    public GameObject warningPanel;
    public GameObject warningPanelPanel;

    public Button YesButton;
    public Button NoButton;

    private bool isPanelOpen = false;

    public void OnButtonClicked()
    {
        Debug.Log("Delete button clicked");
        TogglePanel();
    }

    
    void Start()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }

        YesButton.onClick.AddListener(DeleteHouse);
        NoButton.onClick.AddListener(ClosePanel);
    }

    private void DeleteHouse()
    {
        ClosePanel();
        grid.EraseHouse();
    }
    

    private void TogglePanel()
    {
        if (warningPanel != null)
        {
            isPanelOpen = !isPanelOpen;
            warningPanel.SetActive(isPanelOpen);
        }
    }

    void Update()
    {
        /*
        if (isPanelOpen && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement(warningPanelPanel))
            {
                ClosePanel();
            }
        }
        */
    }

    private void ClosePanel()
    {
        isPanelOpen = false;
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
        
    }
    private bool IsPointerOverUIElement(GameObject targetPanel)
    {
        RectTransform rectTransform = targetPanel.GetComponent<RectTransform>();
        if (rectTransform == null)
            return false;
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);

        return rectTransform.rect.Contains(localMousePosition);
    }
}
