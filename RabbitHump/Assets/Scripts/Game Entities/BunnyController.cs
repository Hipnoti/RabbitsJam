using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public enum BunnyDirectives { Chew, Dig, Hump}
public class BunnyController : GameEntity
{
    public bool CanHump
    {
        get { return humpCooldown <= 0; }
    }

    public GameManager gameManager;
    public GeneralGameSettings gameSettings;
    public NavMeshAgent navMeshAgent;
    public Image actionLoadingImage;
    public TMP_Text currentDirectiveText;
    public BunnyController targetBunny = null;
    public ObjectiveEntity targetObjective;
    public BunnyDirectives currentDirective;
    
    [HideInInspector] public bool goingToHump = false;
    [HideInInspector] public bool humping = false;
    
     private HumpingRole humpingRole = HumpingRole.None;

     private GameAction currentAction;
    private float humpCooldown = 2;

    public void Shove()
    {
        actionLoadingImage.fillAmount = 0;
        humpCooldown = gameManager.generalGameSettings.timeBetweenHumps;
        humping = false;
        targetBunny = null;
        humpingRole = HumpingRole.None;
        
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
        if (goingToHump)
        {
            if (Vector3.Distance(targetBunny.transform.position, transform.position) <= gameSettings.distanceToHump)
            {
                navMeshAgent.isStopped = true;
              //  navMeshAgent.enabled = false;
                EnterHumpState();
              
            }
        }

        if (currentAction != null && currentAction.ActionStarted)
        {
            currentAction.timeLeftForAction -= Time.deltaTime;
            if(humpingRole == HumpingRole.Active)
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

        switch (currentDirective)
        {
            case BunnyDirectives.Chew:
                targetObjective = gameManager.GetRandomObjectiveEntity();
                navMeshAgent.SetDestination(targetObjective.transform.position);
                break;
            case BunnyDirectives.Dig:
                break;
            case BunnyDirectives.Hump:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((goingToHump || humping) || !CanHump)
            return;
        
        if (other.CompareTag("Bunny"))
        {
            targetBunny = other.GetComponent<BunnyController>();
            if(targetBunny.targetBunny != this || !targetBunny.CanHump)
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
     
        navMeshAgent.Move(Vector3.forward * 0.05f);
    }
}
