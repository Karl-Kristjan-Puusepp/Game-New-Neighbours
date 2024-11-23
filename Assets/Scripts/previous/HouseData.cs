using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class HouseData
{
    public TileData[,] tiles = new TileData[4, 3];  // 3x4 grid
}