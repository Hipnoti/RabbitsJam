using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start()
    {
        PhotonNetwork.Instantiate("Prefabs/"+playerPrefab.name, Vector3.zero, quaternion.identity);
    }
}
