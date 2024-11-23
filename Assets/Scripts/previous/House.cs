using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public class House
{
    public HouseTile[,] tiles;  // 3x4 grid of tiles
    public string houseName;
    public int doorAmount;

    public int windowAmount;

    public House(int rows = 4, int cols = 3)
    {
        tiles = new HouseTile[rows, cols];
        houseName = "New House";
        doorAmount = 0;
        windowAmount = 0;
    }

    public void increaseDoors(int door)
    {
        doorAmount += door;
    }
    public void dereaseDoors(int door)
    {
        doorAmount -= door;
    }

    public void addWindows(int window) {
        windowAmount += window;
    }

    public void decreaseWindows(int window) {
        windowAmount -= window;
    }

    public override string ToString() {
        string str = "";
        foreach (var tile in tiles) {
            if (tile == null) 
                str += "NullTile\n";
            else 
                str += tile.ToString() + "\n";
        }
        return str;
    }
}