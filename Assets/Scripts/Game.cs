using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using System.Threading.Tasks;

public class Game : MonoBehaviour
{
    public Button newCustomer;
    public List<CustomerData> Customers;
    public GameObject AllLots;
    public GameObject CustomerPanelObject;
    public GameObject SmallMenuPanel;
    public TextMeshProUGUI GameEndText;
    private CustomerPanel customerPanel;
    public TextMeshProUGUI HappinessText;

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
    }

    private void NextCustomer()
    {
        if (currentCustomerID < Customers.Count) {
            currentCustomer = Customers[currentCustomerID];
            currentCustomerID++;
            CustomerPanelObject.SetActive(true);
            customerPanel.SetCustomerData(currentCustomer);

            SceneData.CurrentCustomerStatic = currentCustomer;

            int count = 0;
            foreach (Transform child in AllLots.transform)
            {
                Button lotButton = child.GetComponent<Button>();
                if (lotButton != null)
                {
                    Debug.Log($"Setting interactable for button: {child.name} to {count < 4}");
                    lotButton.interactable = count < 4; // Enable only the first 4
                    count++;
                }
                else
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

    private async void EndGame()
    {
        int happinessPercentage = (SceneData.happyCustomers * 100) / SceneData.customersTotal;
        string gameEndString = $"You achieved {happinessPercentage}% happiness.\n";
        if (happinessPercentage == 100) gameEndString += "WOW! The townsfolk couldn't be happier!";
        else if (happinessPercentage >= 75) gameEndString += "Impressive! The townsfolk are very happy!";
        else if (happinessPercentage >= 50) gameEndString += "The townsfolk are pleased with the new development.";
        else if (happinessPercentage >= 25) gameEndString += "The townsfolk are somewhat disappointed with your work";
        else gameEndString += "The townsfolk are very displeased with their new properties.";
        GameEndText.text = gameEndString;

        await(Task.Delay(300)); //MenuPanel doesn't open without this wait

        SmallMenuPanel.SetActive(true);
        return;
    }
}
