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
        lotHouse = LotHouseAssigner.GetHouse(row, col);

        if(lotHouse == null)
            lotHouse = new House();

        gridController.DisplayHouse(lotHouse);
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
}