using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonGrid : MonoBehaviour
{
    public Button selectedButton; 

    void Start()
    {
        HighlightButton(selectedButton);
    }

    public void SelectButton(Button button)
    {
        if (selectedButton != null)
        {
            DeselectButton(selectedButton);
        }
        selectedButton = button;
        HighlightButton(selectedButton);
    }

    private void HighlightButton(Button button)
    {
        Color buttonColor = button.GetComponent<Image>().color;
        buttonColor.a = 1f;

        button.GetComponent<Image>().color = buttonColor;
    }

    private void DeselectButton(Button button)
    {
        Color buttonColor = button.GetComponent<Image>().color;
        buttonColor.a = 0.6f;

        button.GetComponent<Image>().color = buttonColor;
    }
}
