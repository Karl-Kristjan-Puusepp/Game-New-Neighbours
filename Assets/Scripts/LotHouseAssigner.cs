using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LotHouseAssigner 
{
    public static House[,] houses = new House[3, 4];
    // Start is called before the first frame update
    
    public static void SetHouse(int row, int col, House house) {
        if (row == -1 || col == -1) return;
        houses[row, col] = house;
    }

    public static House GetHouse(int row, int col) {
        return houses[row, col];
    }
}
