using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRequirements : MonoBehaviour
{
    private string RequirementText;
    private int windowsRequired = -1;
    private int doorsRequired = -1;
    private int widthRequired = -1;
    private int heightRequired = -1;
    private string wallColorRequired = "";
    private string roofColorRequired = "";
    private string locationRequired = "";
    private bool separateHouses = false;

    private string[] wallColors = { "red", "blue", "green", "yellow", "white" , "pink", "black"};
    private string[] roofColors = { "red", "blue", "green", "yellow", "white", "pink", "black" };
    private string[] locations = { "tree", "water", "center" };

    private List<string> selectedRequirements = new List<string>();
    private string[] separateTexts;

    void Start()
    {
        GenerateRandomRequirements();
        
        
    }

    private void GenerateRandomRequirements()
    {
        // Possible requirements
        string[] allRequirements = { "Windows", "Doors", "Width", "Height", "WallColor", "RoofColor", "Location", "SeparateHouses" };

        // Randomly choose 3-4 requirements
        int numRequirements = Random.Range(3, 5); // Randomly choose between 3 and 4

        HashSet<string> chosenRequirements = new HashSet<string>();

        while (chosenRequirements.Count < numRequirements)
        {
            string randomRequirement = allRequirements[Random.Range(0, allRequirements.Length)];
            chosenRequirements.Add(randomRequirement);
        }

        // Process the chosen requirements
        foreach (string requirement in chosenRequirements)
        {
            switch (requirement)
            {
                case "Windows":
                    windowsRequired = Random.Range(0, 9); // Random integer between 1 and 12
                    if (windowsRequired == 1)
                    {
                        separateTexts = new string[] { $"{windowsRequired} window please. ", $"I want my house to have exactly {windowsRequired} window. ", $"Perchance.. could I get {windowsRequired} windows. ", $"{windowsRequired} windows. " };
                    }
                    else
                    {
                        separateTexts = new string[] { $"{windowsRequired} windows please. ", $"I want my house to have exactly {windowsRequired} windows. ", $"Perchance.. could I get {windowsRequired} windows. ", $"{windowsRequired} windows. " };
                    }
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];

                    break;

                case "Doors":
                    doorsRequired = Random.Range(1, 3); // Random integer between 1 and 2
                    if (doorsRequired == 1)  separateTexts = new string[] { $"Only {doorsRequired} door please. ", $"I want my house to have exactly {doorsRequired} door. ", $"Perchance.. could I get {doorsRequired} door. ", $"{doorsRequired} door. " };
                    else separateTexts = new string[] { $"{doorsRequired} doors please. ", $"I want my house to have exactly {doorsRequired} doors. ", $"Perchance.. could I get {doorsRequired} doors. ", $"{doorsRequired} doors. " };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];
                    break;

                case "Width":
                    widthRequired = Random.Range(2, 4); // Random integer between 1 and 3
                    
                    if (widthRequired == 2)  separateTexts = new string[] { $"I don't want too wide of a house... but I don't want it to be too skinny either.. ", $"I want my house to be two blocks wide. " };
                    else if (widthRequired == 3) separateTexts = new string[] { $"I want the widest house possible ", $"I want my house to be very wide. " }; 
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];
                    break;

                case "Height":
                    heightRequired = 4; // Random integer between 1 and 4
                    separateTexts = new string[] { $"I want the tallest house possible. ", $"I want my house to be very tall. ", $"I want my house to be as tall as it possibly can. ", $"My humble abode should have as many floors as possible. " };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];
                    break;

                case "WallColor":
                    wallColorRequired = wallColors[Random.Range(0, wallColors.Length)];
                    separateTexts = new string[] { $"I want the walls of my home to have a little bit of {wallColorRequired}. ", $"My favourite color is {wallColorRequired}, can my walls have {wallColorRequired} on them?. ", $"My mother used to say... if the walls of a house don't have {wallColorRequired}, bad house. " };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];
                    break;

                case "RoofColor":
                    roofColorRequired = roofColors[Random.Range(0, roofColors.Length)];
                    separateTexts = new string[] { $"I want the roof of my home to have a little bit of {roofColorRequired}. ", $"My favourite color is {roofColorRequired}, can my roof have {roofColorRequired} on it?. ", $"My father used to say... if the roof of a house doesn't have {roofColorRequired}, bad house. " };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];
                    break;

                case "Location":
                    locationRequired = locations[Random.Range(0, locations.Length)];
                    if (locationRequired == "tree") separateTexts = new string[] { $"I want to live near {locationRequired}. ", $"I love the forest, I want my house to be as close as possible to trees. ", $"I go walking in the woods sometimes, build my house near the forest! " };
                    else if (locationRequired == "water") separateTexts = new string[] { $"I want to live near {locationRequired}. ", $"I love swimming, I want my house to be as close as possible to a water source. ", $"If you don't build my house near water, I will DESPISE you forever!!!! " };
                    else if (locationRequired == "center") separateTexts = new string[]  { $"I want to live in the {roofColorRequired} of the town. ", $"I want to be far from the forest.. and the water... ", $"Build my house in the center of the town, please! " };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];
                    break;

                case "SeparateHouses":
                    separateHouses = true; // Random boolean
                    separateTexts = new string[] { "I want two houses on the same lot!", "My family would prefer two separate houses", "I cannot stand my wife, build us separate houses!" };
                    RequirementText += separateTexts[Random.Range(0, allRequirements.Length)];

                    break;
            }
        }
        AssignToCustomer();
    }

    private void AssignToCustomer()
    {
        CustomerData customer = SceneData.CurrentCustomerStatic;
        customer.requirementText = RequirementText;

        customer.locationRequired = locationRequired;
        customer.windowsRequired = windowsRequired;
        customer.widthRequired = widthRequired;
        customer.heightRequired = heightRequired;
        customer.doorsRequired = doorsRequired;
        customer.separateHouses = separateHouses;
        customer.roofColorRequired = roofColorRequired;
        customer.wallColorRequired = wallColorRequired;

        SceneData.CurrentCustomerStatic = customer;
    }

    
}
