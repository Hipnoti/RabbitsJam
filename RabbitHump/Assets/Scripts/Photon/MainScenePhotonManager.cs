using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class MainScenePhotonManager : PhotonManager
{
    
    private const string DEFAULT_ROOM_NAME = "GameRoom";

    [SerializeField] private GameManager gameManager;
    private void Awake()
    {
        // if (PhotonNetwork.PhotonServerSettings.StartInOfflineMode)
        // {
        PhotonNetwork.NickName = UnityEngine.Random.Range(1000, 10000).ToString();
            PhotonNetwork.ConnectUsingSettings();
            if (PhotonNetwork.OfflineMode)
            {
                RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2, PlayerTtl = 0, EmptyRoomTtl = 0, IsVisible = true };
                PhotonNetwork.JoinOrCreateRoom(DEFAULT_ROOM_NAME, roomOptions, TypedLobby.Default);
                gameManager.photonView.RPC("InstantiatePlayer", RpcTarget.AllViaServer);
            }
    //    }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2, PlayerTtl = 0, EmptyRoomTtl = 0, IsVisible = true };
        PhotonNetwork.JoinOrCreateRoom(DEFAULT_ROOM_NAME, roomOptions, TypedLobby.Default);
       
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log(newPlayer.NickName + " Has joined");
        if (PhotonNetwork.IsMasterClient)
            gameManager.photonView.RPC("InstantiatePlayer", RpcTarget.AllViaServer);
    }
    
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log(("Room Created"));
        if (PhotonNetwork.PhotonServerSettings.StartInOfflineMode)
        {
            gameManager.photonView.RPC("InstantiatePlayer", RpcTarget.AllViaServer);
        }
    }
    
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(("Joined Room"));
    }
}
