using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Game-New-Neighbors/Customer")]
public class CustomerData : ScriptableObject
{
    
    public string requirementText;
    public int windowsRequired;
    public int doorsRequired;
    public Sprite CustomerSprite;
    public AudioClip CustomerNoise;

}
