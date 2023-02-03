using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MainScenePhotonManager : PhotonManager
{
    private void Awake()
    {
        if (PhotonNetwork.PhotonServerSettings.StartInOfflineMode)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    
}
