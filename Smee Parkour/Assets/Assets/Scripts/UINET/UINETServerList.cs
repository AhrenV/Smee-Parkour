using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using System;
using TMPro;

public class UINETServerList : NetworkBehaviour
{
    public int serverID;
    public UINETServers serverManager;
    [SerializeField] TextMeshProUGUI playerCountUI;
    public override void OnStartClient()
    {
        base.OnStartClient();
    }
    private void Update()
    {
        NetworkConnection[] server = new NetworkConnection[] { };
        bool verified = true;
        try
        {
            server = serverManager.GetLocalServer(serverID);
        }
        catch (Exception e)
        {
            verified = false;
            print(e);
        }
        // If client is connected to a server
        if (verified)
        {
            playerCountUI.text = server.Length + "/4";
        }
    }
}
