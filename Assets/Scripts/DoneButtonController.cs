using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoneButtonController : MonoBehaviour
{
    public HouseBuildingGridController grid;
    public GameObject happyPanel;
    public GameObject doorPanel;

    private bool isPanelOpen = false;

    public void OnButtonClicked() {
        Debug.Log("Done button clicked");
        House currHouse = new House();

        grid.CountThings(currHouse);
        
        if(grid.doorInt > 0) {
            grid.SaveAsActiveHouse();
            TogglePanel();
            Game.currentCustomerID++;
            
        }
        else
        {
            showDoorMessage();
        }
        
        
    }
    
    void Start()
    {
        if (happyPanel != null)
        {
            happyPanel.SetActive(false);
        }
        if (doorPanel != null)
        {
            doorPanel.SetActive(false);
        }
    }

    public void showDoorMessage()
    {   
        doorPanel.SetActive(true);
        doorPanel.GetComponent<CanvasGroup>().alpha = 1.0f;
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade()
    {
        
        CanvasGroup canvasgroup = doorPanel.GetComponent<CanvasGroup>();
        while (canvasgroup.alpha > 0)
        {
            canvasgroup.alpha -= Time.deltaTime / 4;
            yield return null;
        }
        canvasgroup.interactable = false;
        yield return null;
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
