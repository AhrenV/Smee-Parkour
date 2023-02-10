using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Managing.Logging;
using System.Linq;

public class LoadPlayers : NetworkBehaviour
{
    public PartyCreator manager;
    
    
    // Trigger
    [Server(Logging = LoggingType.Off)]
    private void OnTriggerEnter(Collider other)
    {
        NetworkObject nob = other.GetComponent<NetworkObject>();
        if (nob != null)
            manager.LoadLobbyScene("Test Realm", manager.SERVERS[0].ToArray());
    }
}
