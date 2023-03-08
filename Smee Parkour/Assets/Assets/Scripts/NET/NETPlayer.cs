using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class NETPlayer : NetworkBehaviour
{
    [SyncVar]
    public int Health = 100;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DamagePlayer(int damage, NetworkConnection conn = null)
    {
        NETPlayer settings = conn.FirstObject.GetComponent<NETPlayer>();
        settings.Health += damage;
    }


    private void OnCollisionEnter(Collision collision)
    {
        print("col");
        // If collision is from an enemy
        if (!collision.gameObject.GetComponent<NETEnemy>()) { return; }
        print("en");

        Transform enemy = collision.transform;
        int damage = enemy.GetComponent<NETEnemy>().damage;

        DamagePlayer(damage);
    }

}
