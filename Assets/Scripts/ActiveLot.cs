using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLot : MonoBehaviour
{
    // Static property for the house object
    public static int? row;
    public static int? col;

    public static void SetLot(LotController lot)
    {
        row = lot.row;
        col = lot.col;
    }

    public static void ClearLot()
    {
        row = null;
        col = null;
    }
}
