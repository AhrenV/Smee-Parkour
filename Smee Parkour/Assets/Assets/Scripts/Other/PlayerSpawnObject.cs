using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PlayerSpawnObject : NetworkBehaviour
{
    public GameObject enemy;
    public GameObject spawnedObject;
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<PlayerSpawnObject>().enabled = false;
        }
    }

    public void Update()
    {
        if (spawnedObject == null && Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnObject(enemy, transform, this); // sends method to server with the object to spawn, transform, and script that called it  
        }

        if (spawnedObject != null && Input.GetKeyDown(KeyCode.Alpha2))
        {
            DespawnObject(spawnedObject);
        }
    }

    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, PlayerSpawnObject script)
    {
        GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity); // create object from the call of a client
        ServerManager.Spawn(spawned); // spawn object in the server world --> can be seen by all clients
        SetSpawnedObject(spawned, script); // call client function to set spawnedObject parameter to the object for EVERY client so all client know they CANT spawn another object
        // ^ acts as a toggle to prevent more spawned objects as ALL clients not know they cannot spawn another because there is an object in the scripts parameter
        // ^ due to if (spawnedObj = null) in if statement in the Update function --> reason its client is because each client has different parameters to disable
    }

    [ObserversRpc]
    public void SetSpawnedObject(GameObject spawned, PlayerSpawnObject script)
    {
        script.spawnedObject = spawned;
    }


    [ServerRpc(RequireOwnership = false)] // you can spawn objects with 'Ownership'
    public void DespawnObject(GameObject obj)
    {
        ServerManager.Despawn(obj);
    }
}
