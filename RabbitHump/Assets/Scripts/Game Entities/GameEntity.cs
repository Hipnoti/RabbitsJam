using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameEntity : MonoBehaviourPun
{
    public Animator entityAnimator;
    public GameManager gameManager;
    public Outline outlineComp; 
    
    private void OnValidate()
    {
        if (entityAnimator == null)
            entityAnimator = GetComponent<Animator>();
    }
}
