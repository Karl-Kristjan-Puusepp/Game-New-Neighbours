using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public GameObject walls;

    private Image colorButtonImage;
    void Start()
    {
        
        colorButtonImage = GetComponent<Image>();

        // Ensure this button has a Button component and add an onClick listener
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ChangeWallColors);
        }
        else
        {
            Debug.LogError("ColorButton must have a Button component!");
        }
    }

    void ChangeWallColors()
    {
        if (walls == null)
        {
            Debug.LogError("Walls GameObject is not assigned!");
            return;
        }

        Button[] wallButtons = walls.GetComponentsInChildren<Button>();

        foreach (Button wallButton in wallButtons)
        {
            Image wallImage = wallButton.GetComponent<Image>();
            if (wallImage != null && wallImage.sprite.name != "DELETEHOUSE")
            {
                wallImage.color = colorButtonImage.color;
            }
            else
            {
                Debug.LogWarning($"Button {wallButton.name} is missing an Image component.");
            }
        }
    }
}
