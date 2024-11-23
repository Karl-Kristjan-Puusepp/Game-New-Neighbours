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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        if (customerData != null)
        {
            if (panelImage != null)
                panelImage.sprite = customerData.CustomerSprite;

            if (panelText != null)
                panelText.text = customerData.requirementText;


            if (customerData.CustomerNoise != null)
            {
                audioSource.clip = customerData.CustomerNoise; 
                audioSource.playOnAwake = false; 
            }

            audioSource.Play();
        }
    }
}
