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
    private readonly string connector = "-";
    private readonly int partySize = 1;
    
    
    [SyncObject]
    private readonly SyncList<string> SERVERS = new SyncList<string>();

    private void Awake()
    {
        SERVERS.OnChange += _myCollection_OnChange;
    }

    private void _myCollection_OnChange(SyncListOperation op, int index,
         string oldItem, string newItem, bool asServer)
    {
        switch (op)
        {
            case SyncListOperation.Add:
                break;

            case SyncListOperation.RemoveAt:
                break;

            case SyncListOperation.Insert:
                break;

            case SyncListOperation.Set:
                break;

            case SyncListOperation.Clear:
                break;

            case SyncListOperation.Complete:
                break;
        }
    }

    // CREATE A PLAYER LOBBY
    [ServerRpc(RequireOwnership = false)]
    public void CreateServer(NetworkConnection conn = null)
    {
        // Create a new server instance
        string new_server = ""+conn.ClientId;
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
        List<NetworkConnection> server = DecodeString(SERVERS[serverID]);
        if (server.Count >= partySize)
        { print("party full!");  return; }
        server.Add(conn);

        // Syncing Client Settings
        conn.FirstObject.GetComponent<NETClientSettings>().ServerID = serverID;

        // Setting new server value
        SERVERS[serverID] = EncodeString(server);
    }

    /*
    // SEND SERVERID TO CLIENT
    [TargetRpc]
    public void UpdateServerId(int serverID)
    {

    }
    */

    // LOAD LOBBY INTO A DIFFERENT SCENE
    [ServerRpc(RequireOwnership = false)]
    public void LoadLobby(string SCENE_NAME, int serverID)
    {
        print("LL");
        // Grabbing player array via serverID
        //int[] connIDS = SERVERS[serverID].ToArray();
        NetworkConnection[] conns = DecodeString(SERVERS[serverID]).ToArray();

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
        MatchCondition.AddToMatch(serverID, conns);
    }

    // FUNCTIONS FOR OTHER SCRIPTS
    public NetworkConnection[] GetServer(int serverID)
    {
        return DecodeString(SERVERS[serverID]).ToArray();
    }


    // ENCODING METHODS
    private List<NetworkConnection> DecodeString(string str)
    {
        print("SERVER STRING: " + str);
        string[] IDS = str.Split(connector);
        List<NetworkConnection> nobs = new List<NetworkConnection> { };

        foreach (string ID in IDS)
        {
            print(ServerManager.Clients[int.Parse(ID)]);
            nobs.Add(ServerManager.Clients[int.Parse(ID)]);
        }
        return nobs;
    }

    private string EncodeString(List<NetworkConnection> conns)
    {
        List<int> IDs = new List<int> { };
        foreach (NetworkConnection conn in conns)
        {
            IDs.Add(conn.ClientId);
        }
        return string.Join(connector, IDs);
    }

}
