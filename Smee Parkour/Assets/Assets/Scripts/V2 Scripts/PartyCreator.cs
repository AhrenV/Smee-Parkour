using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;


public class PartyCreator : NetworkBehaviour
{
    public UINETServers serverManager;

    // UI FUNCTIONS

    public void ConnectPlayer(int serverID)
    {
        print("connect");
        serverManager.ConnectPlayer(serverID);
    }

    public void CreateServer()
    {
        print("create");
        serverManager.CreateServer();
    }


    public void TeleportServer()
    {
        print("TP!");
        serverManager.LoadLobby("Test Realm", 0);
    }

}
