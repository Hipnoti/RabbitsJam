using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  HumpingRole {None, Passive, Active}
public class HumpGameAction : GameAction
{
    public BunnyController passiveBunny;
    public BunnyController activeBunny;

    public HumpGameAction(float timeUntilAction) : base(timeUntilAction)
    {
        
    }
}
