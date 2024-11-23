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
    public int row;
    public int col;

    private void Awake()
    {
        lotHouse = LotHouseAssigner.GetHouse(row, col);
    }
    private void Start()
    {
        if(this.lotHouse == null)
            this.lotHouse = new House();
        
        lotHouse = LotHouseAssigner.GetHouse(row, col);
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