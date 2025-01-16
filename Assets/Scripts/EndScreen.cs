using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public Button TitleButton;
    public GameObject mainPanel;
    public GameObject menuPanel;
    private bool isPanelOpen = false;

    void Start()
    {
        TitleButton.onClick.AddListener(ToTitleScreen);
        if (mainPanel != null)
        {
            mainPanel.SetActive(false);
        }

    }
    public void ToTitleScreen()
    {
        Debug.Log("Halloo?");
        SceneManager.LoadScene("Title");
    }
    

    public void TogglePanel()
    {
        if (mainPanel != null)
        {
            isPanelOpen = !isPanelOpen;
            mainPanel.SetActive(isPanelOpen);
        }
    }

    void Update()
    {

        if (isPanelOpen && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement(menuPanel, TitleButton))
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

    private bool IsPointerOverUIElement(GameObject targetPanel, Button button)
    {
        RectTransform rectTransform = targetPanel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
            if (rectTransform.rect.Contains(localMousePosition))
            {
                return true; // Pointer is over the panel
            }
        }

        // Check if the pointer is over the button
        if (button != null)
        {
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            if (buttonRectTransform != null)
            {
                Vector2 buttonLocalMousePosition = buttonRectTransform.InverseTransformPoint(Input.mousePosition);
                if (buttonRectTransform.rect.Contains(buttonLocalMousePosition))
                {
                    return true; // Pointer is over the button
                }
            }
        }

        return false; // Pointer is over neither
    }

}
