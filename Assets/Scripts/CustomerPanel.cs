using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerPanel : MonoBehaviour
{
    public Image panelImage; 
    public TextMeshProUGUI panelText; 

    public CustomerData customerData;


    private AudioSource audioSource;

    void Awake()
    {
        if (SceneData.CurrentCustomerStatic != null)
        {
            customerData = SceneData.CurrentCustomerStatic;
            SetCustomerData(customerData);
        }
    }
    void Start()
    {
        if (SceneData.CurrentCustomerStatic != null)
        {
            customerData = SceneData.CurrentCustomerStatic;
            SetCustomerData(customerData);
        }
        audioSource = GetComponent<AudioSource>();  
        
    }
    public void SetCustomerData(CustomerData customerData)
    {
        
        if (customerData != null)
        {
            Debug.Log(customerData);
            this.customerData = customerData;
            if (panelImage != null)
                panelImage.sprite = customerData.CustomerSprite;

            if (panelText != null)
                panelText.text = customerData.requirementText;


            if (customerData.CustomerNoise != null)
            {
                if(audioSource == null)
                {
                    Debug.Log("Audio source is null !?!?!");
                    audioSource = GetComponent<AudioSource>();
                }
                audioSource.clip = customerData.CustomerNoise;
                audioSource.playOnAwake = false;
                audioSource.Play();
                
                
            }

            ;
        }
    }
}
