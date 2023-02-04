using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class GameManager : MonoBehaviourPun
{
    public GeneralGameSettings generalGameSettings;
    public GameObject playerPrefab;
    public BunnyController bunnyPrefab;
    public SpawnPoint spawnPointPrefab;
    public CannonEntity cannon;
    
    public Transform bunnySpawnPoint1;
    public Transform bunnySpawnPoint2;
    public Transform centerPoint;
    
    public List<ObjectiveEntity> defenseObjectives;
    public List<BunnyController> bunniesInGame;

    public Transform firstSpawnPointLocation;
    
    public ObjectiveEntity GetRandomObjectiveEntity()
    {
        return defenseObjectives[UnityEngine.Random.Range(0, defenseObjectives.Count)];
    }

    public void DestroyObjectiveEntity(ObjectiveEntity targetObjective)
    {
        defenseObjectives.Remove(targetObjective);
        PhotonNetwork.Destroy(targetObjective.gameObject);
        
    }

    public void SpawnSpawnPoint(Vector3 point)
    {
        PhotonNetwork.Instantiate("Prefabs/" + spawnPointPrefab.name, point, quaternion.identity);
    }
    
    public BunnyController GetClosestBunny(Vector3 fromPosition, float maxDistance)
    {
        BunnyController closestBunny = null;

        float bestDistance = 9999;
        for (int i = 0; i < bunniesInGame.Count ; i++)
        {
            float currentDistance = Vector3.Distance(fromPosition, bunniesInGame[i].transform.position);
            if (currentDistance <= maxDistance && currentDistance < bestDistance)
            {
                bestDistance = currentDistance;
                closestBunny = bunniesInGame[i];
            }
        }
        return closestBunny;
    }

    [PunRPC]
    public void InstantiatePlayer()
    {
        PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, Vector3.zero, quaternion.identity,
            0, new object[]{Players.Player1 });
        
        PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, Vector3.zero, quaternion.identity,
            0, new object[]{Players.Player2 });
    }

    [PunRPC]
    public void StartGame()
    {
        SpawnSpawnPoint(firstSpawnPointLocation.position);
    }
    
    private void Start()
    {
        // PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, Vector3.zero, quaternion.identity,
        //     0, new object[]{PhotonNetwork.IsMasterClient ? Players.Player1 : Players.Player2});
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     PhotonNetwork.Instantiate("Prefabs/" + bunnyPrefab.name,
        //         bunnySpawnPoint1.position, quaternion.identity);
        //     PhotonNetwork.Instantiate("Prefabs/" + bunnyPrefab.name, 
        //         bunnySpawnPoint2.position, quaternion.identity);
        // }
    }

    private void OnValidate()
    {
        defenseObjectives = FindObjectsOfType<ObjectiveEntity>().ToList();
        
    }
}
