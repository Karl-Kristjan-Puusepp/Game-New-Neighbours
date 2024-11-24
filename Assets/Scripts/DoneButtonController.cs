using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoneButtonController : MonoBehaviour
{
    public HouseBuildingGridController grid;
    public GameObject happyPanel;

    private bool isPanelOpen = false;

    public void OnButtonClicked() {
        Debug.Log("Done button clicked");
        grid.SaveAsActiveHouse();
        TogglePanel();
        
    }
    
    void Start()
    {
        if (happyPanel != null)
        {
            happyPanel.SetActive(false);
        }
    }

    public void TogglePanel()
    {
        if (happyPanel != null)
        {
            isPanelOpen = !isPanelOpen;
            happyPanel.SetActive(isPanelOpen);
        }
    }

    void Update()
    {

        if (isPanelOpen && Input.GetMouseButtonDown(0))
        {
             ClosePanel();
        }
    }

    private void ClosePanel()
    {
        isPanelOpen = false;
        if (happyPanel != null)
        {
            happyPanel.SetActive(false);
        }
        LotHouseAssigner.SetHouse(ActiveLot.row ?? -1, ActiveLot.col ?? -1, ActiveHouse.CurrentHouse);
        ActiveHouse.ClearHouse();
        SceneManager.LoadScene("MapScene");
    }
}
