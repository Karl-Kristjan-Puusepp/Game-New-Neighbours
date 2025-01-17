using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuildSceneController : MonoBehaviour
{
    public TutorialController Tutorial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneData.CurrentCustomerStatic == null) return;
        if (SceneData.CurrentCustomerStatic.id == 0 && !Game.randomRequirements)
        {
            Tutorial.ShowText("Drag and drop tiles to build a house. Click on the available colors to change tile color.");
            Tutorial.ShowNool(0);
        }
        else 
        {
            Tutorial.ShowText("");
            Tutorial.ShowNool(0);
        }
        
    }
}
