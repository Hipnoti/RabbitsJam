using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoadAttribute]
public class PlayStatePhotonSettingsAddOn : MonoBehaviour
{
   
    
    static PlayStatePhotonSettingsAddOn()
    {
        EditorApplication.playModeStateChanged += ChangePhotonServerSettings;
    }

    private static void ChangePhotonServerSettings(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            if (SceneManager.GetActiveScene().name == GameHelper.MAIN_SCENE_NAME)
                PhotonNetwork.PhotonServerSettings.StartInOfflineMode = true;
            else
                PhotonNetwork.PhotonServerSettings.StartInOfflineMode = false;
        }
    }
}
