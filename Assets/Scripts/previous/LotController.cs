using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class LotController : MonoBehaviour
{

    private House lotHouse;
    private Transform gridTransform; // Reference to the Grid object's Transform
    private MiniGridController gridController; // Reference to the Grid's script
    public int row;
    public int col;

    public bool hasHouse = false;

    private void Awake()
    {
        
        lotHouse = LotHouseAssigner.GetHouse(row, col);

        gridTransform = transform.Find("minigrid"); 

        if (gridTransform != null)
        {
            // Get the Grid's script if it exists
            gridController = gridTransform.GetComponent<MiniGridController>();
            gridController.DisplayHouse(lotHouse);

        }
        else
        {
            Debug.LogError("Grid child object not found in Lot prefab!");
        }

    }
    private void Start()
    {   
        SetButtonTransparency(0.7f);
        lotHouse = LotHouseAssigner.GetHouse(row, col);

        if(lotHouse == null)
            lotHouse = new House();

        gridController.DisplayHouse(lotHouse);

        if (LotHouseAssigner.GetHouse(row, col) != null) {
            SetButtonTransparency(0);
        }
    }


    public void OnLotClicked()
    {
        ActiveLot.SetLot(this);
        ActiveHouse.SetHouse(LotHouseAssigner.GetHouse(row, col));
        SceneManager.LoadScene("HouseBuildScene");
    }

    public void SetHouse(House house) {
        this.lotHouse = house;
    }
    public void SetButtonTransparency(float f)
    {
        // Get the Image component attached to the button
        Image buttonImage = GetComponent<Image>();

        if (buttonImage != null)
        {
            // Set the alpha to 0 (fully transparent)
            Color color = buttonImage.color;
            color.a = f; // Alpha = 0 means fully transparent
            buttonImage.color = color;
        }
        else
        {
            Debug.LogError("No Image component found on this Button.");
        }
    }
}