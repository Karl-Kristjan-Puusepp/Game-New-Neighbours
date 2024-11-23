using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoneButtonController : MonoBehaviour
{
    public HouseBuildingGridController grid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClicked() {
        Debug.Log("Done button clicked");
        grid.SaveAsActiveHouse();
        LotHouseAssigner.SetHouse(ActiveLot.row ?? -1, ActiveLot.col ?? -1, ActiveHouse.CurrentHouse);
        ActiveHouse.ClearHouse();
        SceneManager.LoadScene("MapScene");
    }
}
