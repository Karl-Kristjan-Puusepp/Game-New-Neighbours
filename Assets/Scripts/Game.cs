using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using System.Threading.Tasks;
using System;

public class Game : MonoBehaviour
{
    public Button newCustomer;
    public List<CustomerData> Customers;
    public GameObject AllLots;
    public GameObject CustomerPanelObject;
    public GameObject EndMenuPanel;
    public EndScreen endScreen;
    public TextMeshProUGUI GameEndText;
    private CustomerPanel customerPanel;
    public TextMeshProUGUI HappinessText;
    public TextMeshProUGUI ButtonText;

    public List<CustomerData> HappyCustomers;

    public GameObject Villagers;
    public GameObject PartyVillagers;
    public GameObject HappyParty;
    public GameObject Confetti;

    public static List<string> happySpriteNames = new List<string>();
    public static List<string> villageSpriteNames = new List<string>();

    private CustomerData currentCustomer;
    private static int currentCustomerID = 0;

    private void Awake()
    {
        newCustomer.interactable = true;
        customerPanel = CustomerPanelObject.GetComponent<CustomerPanel>();
        if (customerPanel == null)
        {
            Debug.LogError("CustomerPanel script is missing on the assigned CustomerPanelObject.");
        }

        HappinessText.text = $"{SceneData.happyCustomers} / {SceneData.customersTotal}";

        foreach (Transform child in AllLots.transform)
        {
            Button lotButton = child.GetComponent<Button>();
            

            if (lotButton != null)
            {
                lotButton.interactable = false;
            }
        }
        newCustomer.onClick.AddListener(NextCustomer);
        CustomerPanelObject.SetActive(false);
        PartyVillagers.SetActive(false);
        HappyParty.SetActive(false);
        ShowVillagers();
        
    }
    private void Start()
    {
        if (currentCustomerID == Customers.Count)
        {
            ButtonText.text = "See results";
        }
    }

    private void NextCustomer()
    {
        
        if (currentCustomerID < Customers.Count) {
            newCustomer.interactable = false;
            currentCustomer = Customers[currentCustomerID];
            currentCustomerID++;
            CustomerPanelObject.SetActive(true);
            customerPanel.SetCustomerData(currentCustomer);

            SceneData.CurrentCustomerStatic = currentCustomer;

            int count = 0;
            foreach (Transform child in AllLots.transform)
            {
                Button lotButton = child.GetComponent<Button>();
                LotController lotController = lotButton.GetComponent<LotController>();
                if (lotButton != null && !lotController.hasHouse)
                {
                    Debug.Log($"Setting interactable for button: {child.name} to true");
                    lotButton.interactable = true;//count < 4; // Enable only the first 4
                    count++;
                }
                else if (lotController.hasHouse)
                {
                    Debug.Log($"Button: {child.name} already has a house, so it is not interactable");
                }else 
                {
                    Debug.Log($"could not set interactable for button: {child.name}");
                }
            }

        }
        else
        {
            
            EndGame();
        }

        
    }

    public static void AddHappyCustomer(string customerName)
    {
        if (!happySpriteNames.Contains(customerName))
        {
            happySpriteNames.Add(customerName);
        }
    }
    public static void AddCustomer(string customerName)
    {
        if (!villageSpriteNames.Contains(customerName))
        {
            villageSpriteNames.Add(customerName);
        }
    }

    private void ShowVillagers()
    {
        foreach (Transform child in Villagers.transform)
        {
            SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();

            if (childSprite.sprite != null && villageSpriteNames.Contains(childSprite.sprite.name))
            {
                // Enable the image if its sprite name is allowed
                childSprite.enabled = true;
            }
            else
            {
                // Disable the image if not allowed
                childSprite.enabled = false;
            }
        }
        
    }

    private void ShowHappyVillagers()
    {
        Villagers.SetActive(false);
        PartyVillagers.SetActive(true);

        foreach (Transform child in PartyVillagers.transform)
        {
            SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();

            if (childSprite!= null && happySpriteNames.Contains(childSprite.sprite.name))
            {
                // Enable the image if its sprite name is allowed
                childSprite.enabled = true;
            }
            else
            {
                // Disable the image if not allowed
                childSprite.enabled = false;
            }
        }
    }


    private async void EndGame()
    {
        double happinessPercentage = Math.Round((SceneData.happyCustomers * 100) / SceneData.customersTotal, 2);

        string gameEndString = "";

        if (SceneData.customersTotal == 0)
        {
            happinessPercentage = -1;
        }
        else { gameEndString = $"You achieved {happinessPercentage}% happiness.\n"; }

        if (happinessPercentage == 100) gameEndString += "WOW! The townsfolk couldn't be happier!";
        else if (happinessPercentage >= 75) gameEndString += "Impressive! The townsfolk are very happy!";
        else if (happinessPercentage >= 50) gameEndString += "The townsfolk are pleased with the new development.";
        else if (happinessPercentage >= 25) gameEndString += "The townsfolk are somewhat disappointed with your work";
        else if (happinessPercentage == -1) gameEndString += "You didn't build a single house !!!! You sent away all your customers !!! >:( Start over !!!"; 
        else gameEndString += "The townsfolk are very displeased with their new properties.";
        GameEndText.text = gameEndString;
        //Party.FilterImages();
        ShowHappyVillagers();

        if (happinessPercentage >= 50)  HappyParty.SetActive(true);
        else Confetti.SetActive(false);
        
        await(Task.Delay(300)); //MenuPanel doesn't open without this wait

        //SmallMenuPanel.SetActive(true);
        endScreen.TogglePanel();
        return;
    }
}
