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
    public List<BunnyActionChance> bunnyActionChances;
    public float digTime;

    [Header("Spawn")]
    public float spawnTime;
}

[System.Serializable]
public class BunnyActionChance
{
    public BunnyDirectives directive;
    public float chance = 0.5f;
}
