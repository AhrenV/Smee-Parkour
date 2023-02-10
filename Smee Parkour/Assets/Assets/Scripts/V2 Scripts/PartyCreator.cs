using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Component.Observing;
using System.Linq;

public class PartyCreator : NetworkBehaviour
{
    public List<HashSet<NetworkConnection>> SERVERS = new List<HashSet<NetworkConnection>>();
   
    public override void OnStartServer()
    {
        base.OnStartServer();
        HashSet<NetworkConnection> new_server = new HashSet<NetworkConnection>() { }; // test server for development
        SERVERS.Add(new_server); // Adding lobby object into Servers List.
    }

    // Get lobby (list) and load all connection into a scene
    public void LoadLobbyScene(string SCENE_NAME, NetworkConnection[] conns)
    {

        SceneLoadData sld = new SceneLoadData(SCENE_NAME);
        List<NetworkObject> nobs = new List<NetworkObject> {  };

        // Looping through all net connections in lobby
        foreach (var con in conns)
        {
            NetworkObject obj = con.FirstObject.GetComponent<NetworkObject>();
            nobs.Add(obj); // adding player object into array to pass through to new scene
        }

        sld.MovedNetworkObjects = nobs.ToArray(); // carrying all required netobj's over
        sld.ReplaceScenes = ReplaceOption.All;
        SceneManager.LoadConnectionScenes(conns, sld);

        MatchCondition.AddToMatch(conns[0].ClientId, conns);
    }

    // Connect a player to a lobby
    [ServerRpc(RequireOwnership = false)]
    public void ConnectPlayer(NetworkConnection conn = null)
    {
        SERVERS[0].Add(conn); // Adding player to test lobby
    }

    public void OnClick()
    {
        ConnectPlayer();
    }

}
