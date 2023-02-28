using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;


public class NETPlayerLoader : MonoBehaviour
{
    private void Start()
    {
        InstanceFinder.NetworkManager.ClientManager.StartConnection();
    }
}
