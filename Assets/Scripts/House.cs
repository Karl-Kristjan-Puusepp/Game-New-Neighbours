using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public class House
{
    public HouseTile[,] tiles;  // 3x4 grid of tiles
    public string houseName;

    public House(int rows = 3, int cols = 4)
    {
        tiles = new HouseTile[rows, cols];
        houseName = "New House";
    }
}