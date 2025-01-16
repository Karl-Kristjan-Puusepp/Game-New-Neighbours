using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class Party : MonoBehaviour
{
    public static Image[] allImages;

    void Awake()
    {
        allImages = GetComponentsInChildren<Image>();
    }

    public static void FilterImages()
    {
        Debug.Log("FIltering images");
        List<string> allowedSpriteNames = Game.happySpriteNames;
        Debug.Log($"allowed names are {string.Join(", ", allowedSpriteNames)}");
        Debug.Log(allImages);
        // Iterate through all child images
        foreach (Image childImage in allImages)
        {
            Debug.Log(childImage.sprite.name);
            if (childImage != null)
            {
                // Check if the sprite name is in the allowed list
                if (childImage.sprite != null && allowedSpriteNames.Contains(childImage.sprite.name))
                {
                    // Enable the image if its sprite name is allowed
                    childImage.enabled = true;
                }
                else
                {
                    // Disable the image if not allowed
                    childImage.enabled = false;
                }
            }
        }
    }
    }
