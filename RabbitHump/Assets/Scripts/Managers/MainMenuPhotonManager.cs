using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPhotonManager : PhotonManager
{
   private const string DEFAULT_ROOM_NAME = "GameRoom";

   [Header("UI")]
   public Button connectToPhotonButton;
   public TMP_Text connectToPhotonButtonText;
   public TMP_InputField nicknameInputField;
   public TMP_Text connectedPlayersText;

   public override void ConnectToGame()
   {
      PhotonNetwork.NickName = nicknameInputField.text.Trim();
      base.ConnectToGame();
   }

   public override void OnConnectedToMaster()
   {
      base.OnConnectedToMaster();
      PhotonNetwork.AutomaticallySyncScene = true;
      RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2, PlayerTtl = 0, EmptyRoomTtl = 0, IsVisible = true };
      PhotonNetwork.JoinOrCreateRoom(DEFAULT_ROOM_NAME, roomOptions, TypedLobby.Default);
   }

   public override void OnJoinedRoom()
   {
      base.OnJoinedRoom();
      Debug.Log(("Joined Room"));
      connectedPlayersText.text = connectedPlayersText.text.Insert(connectedPlayersText.text.Length, Environment.NewLine + PhotonNetwork.NickName );
      
   }

   public override void OnPlayerEnteredRoom(Player newPlayer)
   {
      base.OnPlayerEnteredRoom(newPlayer);
      Debug.Log(newPlayer.NickName);
      connectedPlayersText.text = connectedPlayersText.text.Insert(connectedPlayersText.text.Length,
         Environment.NewLine + newPlayer.NickName );
      PhotonNetwork.LoadLevel(GameHelper.MAIN_SCENE_NAME);
   }

   public override void OnCreatedRoom()
   {
      base.OnCreatedRoom();
      Debug.Log(("Room Created"));
   }

   private void Start()
   {
      connectedPlayersText.text = string.Empty;
      onConnectionStarted.AddListener(delegate
      {
         connectToPhotonButton.interactable = false;
         connectToPhotonButtonText.text = "Connecting...";
      });
      
   }

  
}
