using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

public class UINETPartyLauncher : NetworkBehaviour
{
    private string targetScene;
    [SerializeField] UINETServers serverManager;

    public void ChangeScene(string SCENE_NAME)
    {
        targetScene = SCENE_NAME;
    }
    public void CreateLobby()
    {
        serverManager.CreateServer(targetScene);
    }
}
