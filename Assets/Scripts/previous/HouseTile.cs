using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public class HouseTile
{
    public Sprite sprite;
    public Color color;

    public HouseTile(Sprite sprite, Color color)
    {
        this.sprite = sprite;
        this.color = color;
    }
}