using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

/// NOTE: Script does nothing, just used for testing.

public class NETPlayerLoader : MonoBehaviour
{
    private void Start()
    {
        InstanceFinder.NetworkManager.ClientManager.StartConnection();
    }
}
