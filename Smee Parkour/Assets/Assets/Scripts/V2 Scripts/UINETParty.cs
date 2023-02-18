using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using System;

// *SIMPLE VERSION WILL NEED REFACTORING + CHANGES * \\

public class UINETParty : NetworkBehaviour
{
    public GameObject PFP;
    public UINETServers serverManager;
    [SerializeField] int memberInt;
    NETClientSettings localSettings;

    public override void OnStartClient()
    {
        base.OnStartClient();
        localSettings = base.LocalConnection.FirstObject.GetComponent<NETClientSettings>();
    }

    private void Update()
    {
        NetworkConnection[] server = new NetworkConnection[] { };
        bool verified = true;
        try
        {
            server = serverManager.GetLocalServer(localSettings.ServerID);
        }
        catch (Exception e)
        {
            verified = false;
            print(e);
        }
        
        if (verified)
        {
            if (server.Length - 1 >= memberInt)
            {
                PFP.SetActive(true);
            }
        }
    }

}
