using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLot : MonoBehaviour
{
    // Static property for the house object
    public static int? row;
    public static int? col;
    public static bool water;
    public static bool forest;
    public static bool center;

    public static void SetLot(LotController lot)
    {
        row = lot.row;
        col = lot.col;
        water = lot.nextToWater;
        forest = lot.nextToForest;
        center = lot.inTheCenter;
    }

    public static void ClearLot()
    {
        row = null;
        col = null;
        water = false;
        forest = false;
        center = false;
    }
}
