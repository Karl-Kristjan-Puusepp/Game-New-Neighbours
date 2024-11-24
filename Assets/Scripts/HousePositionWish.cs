using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePositionWish : Wish
{
    public int rowFrom;
    public int colFrom;
    public int rowTo;
    public int colTo;

    public HousePositionWish(int rowFrom, int colFrom, int rowTo, int colTo)
    {
        this.rowFrom = rowFrom;
        this.colFrom = colFrom;
        this.rowTo = rowTo;
        this.colTo = colTo;
    }
    public override bool isFulfilled(int row, int col)
    {
        if (row >= rowFrom && row < rowTo && col >= colFrom && col < colTo) return true;
        return false;
    }
}
