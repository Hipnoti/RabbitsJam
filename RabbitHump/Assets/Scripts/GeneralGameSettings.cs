using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Yellow Rhymes/General Game Settings", 
    fileName = "General Game Settings")]
public class GeneralGameSettings : ScriptableObject
{
    [Header("Player")]
    public float basePlayerSpeed;
    
    [Header("Bunnies")]
    public float distanceToHump;
    public float humpingTime;
    public float timeBetweenHumps;
    
}

[System.Serializable]
public class PlayerStyle
{
    
}
