using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class TileData
{
    public Sprite sprite;
    public Color color;
    public bool isFilled;

    public TileData(Sprite sprite, Color color, bool isFilled)
    {
        this.sprite = sprite;
        this.color = color;
        this.isFilled = isFilled;
    }
}