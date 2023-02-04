using System;
using System.Collections;
using System.Collections.Generic;
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

    [HideInInspector] public bool goingToHump = false;
    [HideInInspector] public bool humping = false;
    [HideInInspector] public bool onWayToDig = false;

    private HumpingRole humpingRole = HumpingRole.None;

    private GameAction currentAction;
    private float humpCooldown = 2f;

    

    public void Shove()
    {
        navMeshAgent.isStopped = false;
        currentAction = null;
     
        actionLoadingImage.fillAmount = 0;
        if (humping)
        {
            humpCooldown = gameManager.generalGameSettings.timeBetweenHumps;
        }
        humping = false;
        targetBunny = null;
        humpingRole = HumpingRole.None;
         RecalculateDirective();
    }

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

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
        BunnyActionChance chosenActionChance = GameHelper.Choose(gameSettings.bunnyActionChances);
        currentDirective = chosenActionChance.directive;
        currentDirectiveText.text = currentDirective.ToString();
        navMeshAgent.isStopped = false;
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
        }


       
    }

    void EnterHumpState()
    {
        humping = true;
        goingToHump = false;

        currentAction = new GameAction(gameSettings.humpingTime);
        currentAction.onActionEnded.AddListener(EndHumpState);
        currentAction.StartAction();

        currentDirectiveText.text = "Humping!";
    }

    void EndHumpState(GameAction gameAction)
    {
        actionLoadingImage.fillAmount = 0;
        humpCooldown = gameManager.generalGameSettings.timeBetweenHumps;
        humping = false;
        targetBunny = null;
        if (humpingRole == HumpingRole.Active)
        {
            BunnyController instadBunny = Instantiate(gameManager.bunnyPrefab, transform.position, quaternion.identity);
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
    }

    void EndChewState(GameAction gameAction)
    {
        gameManager.DestroyObjectiveEntity(targetObjective);
        RecalculateDirective();
        actionLoadingImage.fillAmount = 0;
      
    }

    void EnterDigState()
    {
        navMeshAgent.isStopped = true;
        onWayToDig = false;
        currentAction = new GameAction(gameSettings.digTime);
        currentAction.onActionEnded.AddListener(EndDigState);
        currentAction.StartAction();
    }

    void EndDigState(GameAction gameAction)
    {
        gameManager.SpawnSpawnPoint(transform.position);
        actionLoadingImage.fillAmount = 0;
        RecalculateDirective();
    }
}