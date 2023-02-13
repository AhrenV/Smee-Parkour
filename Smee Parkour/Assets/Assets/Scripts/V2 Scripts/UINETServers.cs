using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using FishNet.Component.Observing;
using FishNet.Managing.Scened;

public class UINETServers : NetworkBehaviour
{
    // GLOBAL VARIABLES
    public UINETManager UIManager;
    
    
    [SyncObject]
    private readonly SyncList<List<NetworkConnection>> SERVERS = new SyncList<List<NetworkConnection>>();

    private void Awake()
    {
        /* Listening to SyncList callbacks are a
        * little different from SyncVars. */
        SERVERS.OnChange += _myCollection_OnChange;
    }

    /* Like SyncVars the callback offers an asServer option
     * to indicate if the callback is occurring on the server
     * or the client. As SyncVars do, changes have already been
     * made to the collection before the callback occurs. */
    private void _myCollection_OnChange(SyncListOperation op, int index,
         List<NetworkConnection> oldItem, List<NetworkConnection> newItem, bool asServer)
    {
        switch (op)
        {
            /* An object was added to the list. Index
            * will be where it was added, which will be the end
            * of the list, while newItem is the value added. */
            case SyncListOperation.Add:
                break;
            /* An object was removed from the list. Index
            * is from where the object was removed. oldItem
            * will contain the removed item. */
            case SyncListOperation.RemoveAt:
                break;
            /* An object was inserted into the list. Index
            * is where the obejct was inserted. newItem
            * contains the item inserted. */
            case SyncListOperation.Insert:
                break;
            /* An object replaced another. Index
            * is where the object was replaced. oldItem
            * is the item that was replaced, while
            * newItem is the item which now has it's place. */
            case SyncListOperation.Set:
                break;
            /* All objects have been cleared. Index, oldValue,
            * and newValue are default. */
            case SyncListOperation.Clear:
                break;
            /* When complete calls all changes have been
            * made to the collection. You may use this
            * to refresh information in relation to
            * the list changes, rather than doing so
            * after every entry change. Like Clear
            * Index, oldItem, and newItem are all default. */
            case SyncListOperation.Complete:
                break;
        }
    }

    // CREATE A PLAYER LOBBY
    [ServerRpc(RequireOwnership = false)]
    public void CreateServer(NetworkConnection conn = null)
    {
        // Create a new server instance
        List<NetworkConnection> new_server = new List<NetworkConnection>() { conn };
        // Adding instance to SERVERS
        SERVERS.Add(new_server);
        // Get ServerID
        int serverID = SERVERS.IndexOf(new_server);
        // Client function --> Adding frame to Server List for clients.
        UIManager.UIAddServer(serverID);
        print("Server created with ID: " + serverID);
    }


    // CONNECT A PLAYER TO AN EXSTING LOBBY
    [ServerRpc(RequireOwnership = false)]
    public void ConnectPlayer(int serverID, NetworkConnection conn = null)
    {
        // Adding player to SERVERS datatable
        SERVERS[serverID].Add(conn);
    }


    // LOAD LOBBY INTO A DIFFERENT SCENE
    [ServerRpc(RequireOwnership = false)]
    public void LoadLobby(string SCENE_NAME, int serverID)
    {
        // Grabbing player array via serverID
        NetworkConnection[] conns = SERVERS[serverID].ToArray();
        // Make SLD object
        SceneLoadData sld = new SceneLoadData(SCENE_NAME);
        // Create List of NetworkObjects to pass through
        List<NetworkObject> nobs = new List<NetworkObject> { };

        // Looping through all client connections
        foreach (var con in conns)
        {
            // Retrieving NetOb component
            NetworkObject obj = con.FirstObject.GetComponent<NetworkObject>();
            // Adding Nob to List to pass through
            nobs.Add(obj);
        }
        // Add Nobs to moved + convert to array
        sld.MovedNetworkObjects = nobs.ToArray();
        // Replace options
        sld.ReplaceScenes = ReplaceOption.All;
        // Load players + nobs into new scene
        SceneManager.LoadConnectionScenes(conns, sld);

        // Add to match condition
        MatchCondition.AddToMatch(conns[0].ClientId, conns);
    }

}
