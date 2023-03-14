using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

/// NOTE: This script is used as a component which will eventually store all the important client information that needs to be accessed by the server. In the future this will include the players inventory, ect.

public class NETClientSettings : NetworkBehaviour
{
    // Holds the ServerID of the local player.
    [SyncVar]
    public int ServerID = -1;
}
