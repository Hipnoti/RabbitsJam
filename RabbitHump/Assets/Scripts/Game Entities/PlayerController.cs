using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum Players {Player1, Player2}
public class PlayerController : GameEntity, IPunInstantiateMagicCallback
{
    private const string ANIMATOR_STATE_PARAMETER_NAME = "Current State";
    private const int IDLE_ANIM = 1;
    private const int WALK_ANIM = 2;
    private const int FIX_ANIM = 3;
    private const int HOLD_ANIM = 4;
    
    private readonly Vector3 upVector = Vector3.forward;
    private readonly Vector3 downVector = Vector3.back;
    private readonly Vector3 leftVector = Vector3.left;
    private readonly Vector3 rightVector = Vector3.right;
    private readonly Vector3 zeroVector = Vector3.zero;
    
    public Players playerNumber;
    public BunnyController carriedBunny;
    public BunnyController bunnyInRange;
    [SerializeField] private GameInputSettings inputSettings;
    [SerializeField] private GeneralGameSettings gameSettings;
    // Start is called before the first frame update

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        playerNumber = (Players)info.photonView.InstantiationData[0];
    }

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    void Update()
    {
        if (photonView.IsMine && GameHelper.acceptInput)
        {
            Vector3 moveVector = Vector3.zero;
            moveVector += Input.GetKey(inputSettings.upKey) ? upVector : zeroVector;
            moveVector += Input.GetKey(inputSettings.downKey) ? downVector : zeroVector;
            moveVector += Input.GetKey(inputSettings.leftKey) ? leftVector : zeroVector;
            moveVector += Input.GetKey(inputSettings.rightKey) ? rightVector : zeroVector;

            if (carriedBunny == null)
            {
                if (moveVector != Vector3.zero)
                    entityAnimator.SetInteger(ANIMATOR_STATE_PARAMETER_NAME, WALK_ANIM);
                else
                {
                    entityAnimator.SetInteger(ANIMATOR_STATE_PARAMETER_NAME, IDLE_ANIM);
                }
            }

            transform.Translate(moveVector * (Time.deltaTime * gameSettings.basePlayerSpeed));

            Vector3 v = transform.position - gameManager.centerPoint.position;
            v = Vector3.ClampMagnitude(v, 4.5f);
            transform.position = gameManager.centerPoint.position + v;
           
            if (carriedBunny != null)
            {
                if (Vector3.Distance(transform.position, gameManager.cannon.transform.position) <=
                    gameSettings.useDistance)
                {
                    gameManager.cannon.ToggleOutline(true);
                    if (Input.GetKeyDown(inputSettings.useKey))
                    {
                        carriedBunny = null;
                        entityAnimator.SetInteger(ANIMATOR_STATE_PARAMETER_NAME, IDLE_ANIM);
                        gameManager.cannon.LaunchBunny();
                    }
                }
            }
            else
            {
                if (gameManager.cannon.outlineComps[0].enabled)
                    gameManager.cannon.ToggleOutline(false);
                BunnyController newBunnyInRange = gameManager.GetClosestBunny(transform.position, gameSettings.useDistance);
                if (bunnyInRange != null && newBunnyInRange != bunnyInRange)
                    bunnyInRange.ToggleOutline(false);
                bunnyInRange = newBunnyInRange;
                if(bunnyInRange != null)
                    bunnyInRange.ToggleOutline(true);
                if (Input.GetKeyDown(inputSettings.useKey))
                {
                  
                  
                    if (bunnyInRange != null && carriedBunny == null)
                    {
                        carriedBunny = bunnyInRange;
                        carriedBunny.gameObject.SetActive(false);
                        entityAnimator.SetInteger(ANIMATOR_STATE_PARAMETER_NAME, HOLD_ANIM);
                       
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bunny Human Interaction"))
        {
            Debug.Log(("Stop it bunny!"));
            BunnyController bunnyController = other.GetComponentInParent<BunnyController>();
            if(bunnyController == null)
                return;
            
            if(bunnyController.humping)
                bunnyController.targetBunny.Shove();
            bunnyController.Shove();
      
        }

       
    }

    
}
