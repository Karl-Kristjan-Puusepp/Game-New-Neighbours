using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActiveHouse
{
    // Static property for the house object
    public static House CurrentHouse { get; private set; }

    public static void SetHouse(House house)
    {
        if (house == null) return; 
        Debug.Log("Active House set to: " + house.ToString());
        CurrentHouse = house;
    }

    public static void ClearHouse()
    {
        CurrentHouse = null;
    }
}
