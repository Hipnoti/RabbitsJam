using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public enum BunnyDirectives
{
    None,
    Chew,
    Dig,
    Hump
}

public class BunnyController : GameEntity
{
    private const string ANIMATOR_STATE_PARAMETER_NAME = "Current State";
    private const int WALK_ANIM = 1;
    private const int DIG_ANIM = 2;
    private const int CHEW_ANIM = 3;
    private const int FLY_ANIM = 4;
    private const int HUMP_ANIM = 5;
    
    public bool CanHump
    {
        get { return humpCooldown <= 0; }
    }

  
    public GeneralGameSettings gameSettings;
    public NavMeshAgent navMeshAgent;
    public Image actionLoadingImage;
    public TMP_Text currentDirectiveText;
    public BunnyController targetBunny = null;
    public ObjectiveEntity targetObjective;
    public Vector3 targetDigPosition;
    public BunnyDirectives currentDirective;
    public ParticleSystem diggingParticle;
    
    public GameObject canvasObject;

    [Header("Audio")]
    public AudioSource bornAudio;
    public AudioSource slapAudio;
    public AudioSource shootAudio;
    public AudioSource digAudio;
    public AudioSource humpAudio;
    
    [HideInInspector] public bool goingToHump = false;
    [HideInInspector] public bool humping = false;
    [HideInInspector] public bool onWayToDig = false;

    private HumpingRole humpingRole = HumpingRole.None;

    private GameAction currentAction;
    private float humpCooldown = 2f;
    

    public void Shove(bool withAudio = true)
    {
        navMeshAgent.enabled = true;
        navMeshAgent.isStopped = false;
        currentAction = null;
     
        actionLoadingImage.fillAmount = 0;
        if (humping)
        {
            Debug.Log("Resseting cooldown");
            humpCooldown = gameManager.generalGameSettings.timeBetweenHumps;
        }
        humping = false;
        targetBunny = null;
        humpingRole = HumpingRole.None;
         RecalculateDirective();
         
         // if(withAudio)
         //  slapAudio.Play();
    }

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        
        
        gameManager.bunniesInGame.Add(this);
        navMeshAgent.updateRotation = false;
        actionLoadingImage.fillAmount = 0;
        RecalculateDirective();
    }

    private void Update()
    {
        humpCooldown -= Time.deltaTime;
        if (goingToHump && targetBunny != null)
        {
            if (Vector3.Distance(targetBunny.transform.position, transform.position) <= gameSettings.distanceToHump)
            {
                navMeshAgent.isStopped = true;
                //  navMeshAgent.enabled = false;
                EnterHumpState();
            }
        }

        if (currentDirective == BunnyDirectives.Chew)
        {
            if (targetObjective == null)
            {
                RecalculateDirective();
            }
        }

        if (currentDirective == BunnyDirectives.Dig && onWayToDig)
        {
            if (Vector3.Distance(targetDigPosition, transform.position) <= 0.05f)
            {
                
                EnterDigState();
                
                Debug.Log(("Digging"));
            }
        }
        
        if (currentAction != null && currentAction.ActionStarted)
        {
            currentAction.timeLeftForAction -= Time.deltaTime;
          //  if (humping && humpingRole == HumpingRole.Active)
                actionLoadingImage.fillAmount = currentAction.PercentComplete;
            if (currentAction.timeLeftForAction <= 0)
            {
                currentAction.EndAction();
            }
        }
    }

    private void RecalculateDirective()
    {
        digAudio.Stop();
        BunnyActionChance chosenActionChance = GameHelper.Choose(gameSettings.bunnyActionChances);
        currentDirective = chosenActionChance.directive;
        currentDirectiveText.text = currentDirective.ToString();
        navMeshAgent.enabled = true;
        navMeshAgent.isStopped = false;
        diggingParticle.Stop();
        entityAnimator.SetInteger(ANIMATOR_STATE_PARAMETER_NAME, WALK_ANIM);
        switch (currentDirective)
        {
            case BunnyDirectives.Chew:
                if(gameManager.defenseObjectives.Count == 0)
                    return;;
                if (gameManager.defenseObjectives.Count > 1)
                {
                    ObjectiveEntity newTargetObjective;
                    do
                    {
                        newTargetObjective = gameManager.GetRandomObjectiveEntity();
                    } while (newTargetObjective == targetObjective);

                    targetObjective = newTargetObjective;
                }
                else
                 targetObjective = gameManager.GetRandomObjectiveEntity();
                
                navMeshAgent.SetDestination(targetObjective.transform.position);
                break;
            case BunnyDirectives.Dig:
                for (int i = 0; i < 30; i++)
                {
                    Vector3 result = Vector3.zero;
                    Vector3 randomPoint = gameManager.centerPoint.position + Random.insideUnitSphere * 4.5f;
                    NavMeshHit hit;
                    onWayToDig = true;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                        result = hit.position;
                        targetDigPosition = result;
                        navMeshAgent.SetDestination(result);
                    }
                }
                break;
            case BunnyDirectives.Hump:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objective"))
        {
            if (currentDirective == BunnyDirectives.Chew && other.GetComponent<ObjectiveEntity>() == targetObjective)
            {
                Debug.Log("Reached Target");
               EnterChewState();
            }
        }
        
        if ((goingToHump || humping) || !CanHump)
            return;

        if (other.CompareTag("Bunny"))
        {
            targetBunny = other.GetComponent<BunnyController>();
            if (targetBunny.targetBunny != this || !targetBunny.CanHump)
                return;

            if (humpingRole == HumpingRole.None)
            {
                humpingRole = Random.value >= 0.5f ? HumpingRole.Active : HumpingRole.Passive;
                targetBunny.humpingRole = humpingRole == HumpingRole.Active ? HumpingRole.Passive : HumpingRole.Active;
            }

            Vector3 targetPosition = (transform.position + other.transform.position) / 2;
            navMeshAgent.SetDestination(targetPosition);
            goingToHump = true;
            Debug.Log("going to hump!!!", gameObject);
        }


       
    }

    void EnterHumpState()
    {
        humping = true;
        goingToHump = false;
        navMeshAgent.enabled = false;
        currentAction = new GameAction(gameSettings.humpingTime);
        currentAction.onActionEnded.AddListener(EndHumpState);
        currentAction.StartAction();

        if(humpingRole == HumpingRole.Active)
            entityAnimator.SetInteger(ANIMATOR_STATE_PARAMETER_NAME, HUMP_ANIM);
        else
            entityAnimator.SetInteger(ANIMATOR_STATE_PARAMETER_NAME, WALK_ANIM);
        currentDirectiveText.text = "Humping!";
        
        humpAudio.Play();
    }

    void EndHumpState(GameAction gameAction)
    {
        navMeshAgent.enabled = true;
        actionLoadingImage.fillAmount = 0;
        humpCooldown = gameManager.generalGameSettings.timeBetweenHumps;
        humping = false;
        targetBunny = null;
        if (humpingRole == HumpingRole.Active)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("Prefabs/" +gameManager.bunnyPrefab.name, transform.position,
                    quaternion.identity);
            }
        }

        humpingRole = HumpingRole.None;
        currentAction = null;
        navMeshAgent.Move(Vector3.forward * 0.05f);
    }

    void EnterChewState()
    {
        navMeshAgent.isStopped = true;
        currentAction = new GameAction(3f);
        currentAction.onActionEnded.AddListener(EndChewState);
        currentAction.StartAction();
        entityAnimator.SetInteger(ANIMATOR_STATE_PARAMETER_NAME, CHEW_ANIM);
    }

    void EndChewState(GameAction gameAction)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameManager.DestroyObjectiveEntity(targetObjective);
        }

        RecalculateDirective();
        actionLoadingImage.fillAmount = 0;
      
    }

    void EnterDigState()
    {
        if(!onWayToDig)
            return;;
        navMeshAgent.isStopped = true;
        onWayToDig = false;
        diggingParticle.Play();
        currentAction = new GameAction(gameSettings.digTime);
        currentAction.onActionEnded.AddListener(EndDigState);
        currentAction.StartAction(); 
        entityAnimator.SetInteger(ANIMATOR_STATE_PARAMETER_NAME, DIG_ANIM);
        digAudio.Play();
    }

    void EndDigState(GameAction gameAction)
    {
        gameManager.SpawnSpawnPoint(transform.position);
        diggingParticle.Stop();
        actionLoadingImage.fillAmount = 0;
        RecalculateDirective();
        digAudio.Stop();
    }
}