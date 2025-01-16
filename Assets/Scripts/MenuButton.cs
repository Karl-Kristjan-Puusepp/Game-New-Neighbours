using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public GameObject mainPanel; 
    public GameObject menuPanel;
    public Button ContinueButton;

    private bool isPanelOpen = false; 

    void Start()
    {
        if (mainPanel != null)
        {
            mainPanel.SetActive(false); 
        }
        ContinueButton.onClick.AddListener(ClosePanel);
    }

    public void TogglePanel()
    {
        Debug.Log("Click clikc click");
        if (mainPanel != null)
        {
            Debug.Log("whaaaatt");
            isPanelOpen = !isPanelOpen; 
            mainPanel.SetActive(isPanelOpen); 
        }
    }

    void Update()
    {
        
        if (isPanelOpen && Input.GetMouseButtonDown(0))
        {
            //if (!IsPointerOverUIElement(menuPanel))
            {
                ClosePanel();
            }
        }
    }

    private void ClosePanel()
    {
        isPanelOpen = false;
        if (mainPanel != null)
        {
            mainPanel.SetActive(false);
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
