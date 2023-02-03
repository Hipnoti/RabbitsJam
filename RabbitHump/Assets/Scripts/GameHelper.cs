using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper 
{
    public const string MAIN_SCENE_NAME = "MainScene";
    public static bool acceptInput = true;
    
    public static BunnyActionChance Choose (List<BunnyActionChance> probs)
    {
        float total = 0;

        for (int i = 0; i < probs.Count; i++)
        {
            total += probs[i].chance;
        }

        float randomPoint = UnityEngine.Random.value * total;

        for (int i= 0; i < probs.Count; i++)
        {
            if (randomPoint < probs[i].chance)
            {
                return probs[i];
            }
            else
            {
                randomPoint -= probs[i].chance;
            }
        }

        return probs[probs.Count - 1];
    }
}
