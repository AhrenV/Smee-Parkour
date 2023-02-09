using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Managing.Logging;
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

    // Connect a player to a lobby
    [ServerRpc(RequireOwnership = false)]
    public void ConnectPlayer(NetworkConnection conn = null)
    {
        print(conn.ClientId);
        SERVERS[0].Add(conn); // Adding player to lobby
    }

    // Get lobby (list) and load all connection into a scene
    public void LoadLobbyScene(string SCENE_NAME, NetworkConnection[] conns)
    {
        SceneLoadData sld = new SceneLoadData(SCENE_NAME);
        NetworkObject[] nobs = new NetworkObject[] { };

        sld.MovedNetworkObjects = nobs; // carrying all required netobj's over
        sld.ReplaceScenes = ReplaceOption.None;
        SceneManager.LoadConnectionScenes(conns, sld);
    }
    [ServerRpc(RequireOwnership =false)]

    public void OnClick()
    {
        ConnectPlayer();
    }

}
