using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class TutorialController : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; 
    public GameObject nool1;
    public GameObject nool2;

    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        nool1.SetActive(false);
        nool2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowNool(int i) {
        if (i == 0) {
            nool1.SetActive(false);
            nool2.SetActive(false);
        }
        if (i == 1) 
        {
            nool1.SetActive(true);
            nool2.SetActive(false);
        }
        else if(i == 2) 
        {
            nool2.SetActive(true);
            nool1.SetActive(false);
        }

    }

    public void ShowText(string str){
        if (str == "") 
        {
            img.gameObject.SetActive(false);
        }
        tutorialText.text = str;
    }
}
