using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAmountWish : Wish
{
    public int amount;

    public DoorAmountWish(int amount)
    {
        this.amount = amount;
    }
    public override bool isFulfilled(int row, int col)
    {
        House house = LotHouseAssigner.GetHouse(row, col);
        if (house.doorAmount == amount) return true;
        return false;
    }
}
