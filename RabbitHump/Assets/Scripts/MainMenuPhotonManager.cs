using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPhotonManager : PhotonManager
{
   public Button connectToPhotonButton;
   public TMP_Text connectToPhotonButtonText;
   private void Start()
   {
      onConnectionStarted.AddListener(delegate
      {
         connectToPhotonButton.interactable = false;
         connectToPhotonButtonText.text = "Connecting...";
      });
      
   }
}
