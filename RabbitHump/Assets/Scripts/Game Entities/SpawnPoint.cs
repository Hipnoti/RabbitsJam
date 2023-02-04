using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPoint : GameEntity
{
    [SerializeField] private GeneralGameSettings generalGameSettings;

    private GameAction spawnAction = null;

    private void Start()
    {
        spawnAction = new GameAction(generalGameSettings.spawnTime);
        spawnAction.onActionEnded.AddListener(Spawn);
        spawnAction.StartAction();
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void Update()
    {
        spawnAction.timeLeftForAction -= Time.deltaTime;
        if (spawnAction.timeLeftForAction <= 0)
        {
            spawnAction.EndAction();
            spawnAction.timeLeftForAction = spawnAction.totalTimeForAction;
            spawnAction.StartAction();
        }
    }

    private void Spawn(GameAction gameAction)
    {
        PhotonNetwork.Instantiate("Prefabs/"+gameManager.bunnyPrefab.name, transform.position, Quaternion.identity);
    }
}
