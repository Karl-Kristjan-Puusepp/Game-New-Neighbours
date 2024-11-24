using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Button newCustomer;
    public List<CustomerData> Customers;
    public GameObject AllLots;
    public GameObject CustomerPanelObject;
    private CustomerPanel customerPanel;

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

    private void EndGame()
    {
        return;
    }
}
