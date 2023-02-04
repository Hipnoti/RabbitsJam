using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class GameManager : MonoBehaviour
{
    public GeneralGameSettings generalGameSettings;
    public GameObject playerPrefab;
    public BunnyController bunnyPrefab;

    public Transform bunnySpawnPoint1;
    public Transform bunnySpawnPoint2;

    public List<ObjectiveEntity> defenseObjectives;
    
    

    public ObjectiveEntity GetRandomObjectiveEntity()
    {
        return defenseObjectives[UnityEngine.Random.Range(0, defenseObjectives.Count)];
    }

    public void DestroyObjectiveEntity(ObjectiveEntity targetObjective)
    {
        defenseObjectives.Remove(targetObjective);
        PhotonNetwork.Destroy(targetObjective.gameObject);
        
    }
    private void Start()
    {
        PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, Vector3.zero, quaternion.identity,
            0, new object[]{PhotonNetwork.IsMasterClient ? Players.Player1 : Players.Player2});
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
