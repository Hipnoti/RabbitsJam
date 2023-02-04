using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class GameEntity : MonoBehaviourPun
{
    public Animator entityAnimator;
    public GameManager gameManager;
    public List<Outline> outlineComps; 
    
    private void OnValidate()
    {
        if (entityAnimator == null)
            entityAnimator = GetComponent<Animator>();
     //   if (outlineComps == null)
            outlineComps = GetComponentsInChildren<Outline>().ToList();
        
    }

    public void ToggleOutline(bool isActive)
    {
        foreach (Outline outline in outlineComps)
        {
            outline.enabled = isActive;
        }
    }
}
