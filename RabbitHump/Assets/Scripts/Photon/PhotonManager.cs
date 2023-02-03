using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public abstract class PhotonManager : MonoBehaviourPunCallbacks
{
      protected UnityEvent onConnectionStarted = new UnityEvent();
      
      public virtual void ConnectToGame()
      {
            onConnectionStarted.Invoke();
            PhotonNetwork.ConnectUsingSettings();
      }

      void OnDisconnected()
      {
            Debug.LogError("Disconnected");
      }
      
      public override void OnConnectedToMaster()
      {
            base.OnConnectedToMaster();
            Debug.Log("Connected");
      }
}
