using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowAmountWish : Wish
{
    public int amount;

    public WindowAmountWish(int amount)
    {
        this.amount = amount;
    }
    public override bool isFulfilled(int row, int col)
    {
        House house = LotHouseAssigner.GetHouse(row, col);
        if (house.windowAmount == amount) return true;
        return false;
    }
}
