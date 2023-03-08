using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class NETClientSettings : NetworkBehaviour
{
    [SyncVar]
    public int ServerID = -1;
}
