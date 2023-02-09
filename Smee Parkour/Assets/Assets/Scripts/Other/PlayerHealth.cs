using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar] public int health = 10; // with SyncVar any time the health is changed on the server, the new value will be sent to the clients.
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<PlayerSpawnObject>().enabled = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            UpdateHealth(this, -1);
        }
    }
    [ServerRpc]
    public void UpdateHealth(PlayerHealth script, int amount)
    {
        script.health += amount;
    }
}
