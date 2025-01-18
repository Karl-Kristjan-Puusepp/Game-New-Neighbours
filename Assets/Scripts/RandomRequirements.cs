using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRequirements : MonoBehaviour
{
    private static string RequirementText;
    private static int windowsRequired = -1;
    private static int doorsRequired = -1;
    private static int widthRequired = -1;
    private static int heightRequired = -1;
    private static string wallColorRequired = "";
    private static string roofColorRequired = "";
    private static string locationRequired = "";
    private static bool separateHouses = false;

    private static string[] wallColors = { "red", "blue", "green", "yellow", "white" , "pink", "black"};
    private static string[] roofColors = { "red", "blue", "green", "yellow", "white", "pink", "black" };
    private static string[] locations = { "tree", "water", "center" };

    private static List<string> selectedRequirements = new List<string>();
    private static string[] separateTexts;


    public static void GenerateRandomRequirements(int customerID)
    {
        RequirementText = "";
        windowsRequired = -1;
        doorsRequired = -1;
        widthRequired = -1;
        heightRequired = -1;
        wallColorRequired = "";
        roofColorRequired = "";
        locationRequired = "";
        separateHouses = false;

        // Possible requirements
        string[] allRequirements = { "Windows", "Doors", "Width", "Height", "WallColor", "RoofColor", "Location", "SeparateHouses" };

        // Randomly choose 3-4 requirements
        int numRequirements = Random.Range(2, 5); // Randomly choose between 2 and 4

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
                        separateTexts = new string[] { $"{windowsRequired} window please. ", $"I want my house to have exactly {windowsRequired} window. ", $"Perchance.. could I get {windowsRequired} window. ", $"{windowsRequired} window. ", "2 windows! ..erm.. actually, only one window" };
                    }
                    else
                    {
                        separateTexts = new string[] { $"{windowsRequired} windows please. ", $"I want my house to have exactly {windowsRequired} windows. ", $"Perchance.. could I get {windowsRequired} windows. ", $"{windowsRequired} windows. ", $"windows: {windowsRequired}. ", $"1 window for my room, {windowsRequired-1} more for other rooms"  };
                    }
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];

                    break;

                case "Doors":
                    if (chosenRequirements.Contains("SeparateHouses")) break;
                    else doorsRequired = Random.Range(1, 3); // Random integer between 1 and 2

                    if (doorsRequired == 1)  separateTexts = new string[] { $"Only {doorsRequired} door please. ", $"I want my house to have exactly {doorsRequired} door. ", $"Perchance.. could I get {doorsRequired} door. ", $"{doorsRequired} door. " };
                    else separateTexts = new string[] { $"{doorsRequired} doors please. ", $"I want my house to have exactly {doorsRequired} doors. ", $"Perchance.. could I get {doorsRequired} doors. ", $"{doorsRequired} doors. ", $"doors: {doorsRequired}. ", "1 door for me, 1 for my wife. ", "1 door for me and 1 more for all my friends. ", "1 door to go in the house, another door to come out the house" };
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
                    separateTexts = new string[] { $"I want the walls of my home to have a little bit of {wallColorRequired}. ", $"My favourite color is {wallColorRequired}, can my walls have {wallColorRequired} on them?. ", $"My mother used to say... the walls of a house must always have {wallColorRequired}. ", $"{wallColorRequired} walls. ", $"{wallColorRequired} walls. ", $"{wallColorRequired} walls. ", $"walls: {wallColorRequired}. ", $"walls: {wallColorRequired}. ", $"walls: {wallColorRequired}. " };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];
                    break;

                case "RoofColor":
                    roofColorRequired = roofColors[Random.Range(0, roofColors.Length)];
                    separateTexts = new string[] { $"I want the roof of my home to have a little bit of {roofColorRequired}. ", $"My favourite color is {roofColorRequired}, can my roof have {roofColorRequired} on it?. ", $"My father used to say... a house without a {roofColorRequired} roof is no good!. ", $"{roofColorRequired} roof.", $"{roofColorRequired} roof.", $"roof: {roofColorRequired}.", $"roof: {roofColorRequired}." };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];
                    break;

                case "Location":
                    locationRequired = locations[Random.Range(0, locations.Length)];
                    if (locationRequired == "tree") separateTexts = new string[] { $"I want to live near {locationRequired}. ", $"I love the forest, I want my house to be as close as possible to trees. ", $"I go walking in the woods sometimes, build my house near the forest! " };
                    else if (locationRequired == "water") separateTexts = new string[] { $"I want to live near {locationRequired}. ", $"I love swimming, I want my house to be as close as possible to a water source. ", $"If you don't build my house near water, I will DESPISE you forever!!!! " };
                    else if (locationRequired == "center") separateTexts = new string[]  { $"I want to live in the {locationRequired} of the town. ", $"I want to be far from the forest.. and the water... ", $"Build my house in the center of the town, please! " };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];
                    break;

                case "SeparateHouses":

                    if (chosenRequirements.Contains("Width")) break;

                    doorsRequired = 2;
                    separateTexts = new string[] { $"{doorsRequired} doors please. ", $"I want my house to have exactly {doorsRequired} doors. ", $"Perchance.. could I get {doorsRequired} doors. ", $"{doorsRequired} doors. " };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];

                    separateHouses = true; // Random boolean
                    separateTexts = new string[] { "I want two houses on the same lot! ", "My family would prefer two separate houses ", "I cannot stand my wife, build us separate houses! ", "I love my wife so much, I want to give her two houses instead of one! " };
                    RequirementText += separateTexts[Random.Range(0, separateTexts.Length)];

                    break;
            }
        }
        AssignToCustomer(customerID);
    }

    public static void AssignToCustomer(int customerID)
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
        customer.id = customerID;

        SceneData.CurrentCustomerStatic = customer;
    }

    
}
