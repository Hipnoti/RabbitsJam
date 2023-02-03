using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameAction
{
    [HideInInspector] public UnityEvent<GameAction> onActionStarted = new UnityEvent<GameAction>();
    [HideInInspector] public UnityEvent<GameAction> onActionEnded = new UnityEvent<GameAction>();

    public bool ActionStarted => actionStarted;

    public float PercentComplete => (totalTimeForAction - timeLeftForAction) / totalTimeForAction;

    public float timeLeftForAction;
    private float totalTimeForAction;
    
    private bool actionStarted = false;
  

    public GameAction()
    {
        
    }

    public GameAction(float timeUntilAction)
    {
        totalTimeForAction = timeUntilAction;
        timeLeftForAction = timeUntilAction;
    }
    
    public void StartAction()
    {
        actionStarted = true;
        onActionStarted.Invoke(this);
    }

    public void EndAction()
    {
        actionStarted = false;
        onActionEnded.Invoke(this);
    }
}
