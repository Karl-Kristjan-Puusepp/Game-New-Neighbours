using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Game-New-Neighbors/Customer")]
public class CustomerData : ScriptableObject
{
    
    public string requirementText;
    public int windowsRequired=-1;
    public int doorsRequired=-1;
    public Sprite CustomerSprite;
    public AudioClip CustomerNoise;
    public string CustomerName;
}
