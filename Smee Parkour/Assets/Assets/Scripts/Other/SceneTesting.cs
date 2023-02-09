using FishNet;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet.Managing.Logging;
using FishNet.Connection;
using System.Linq;

public class SceneTesting : NetworkBehaviour
{
    public const string SCENE_NAME = "Test Realm";
    public PartyCreator manager;

    [Server(Logging = LoggingType.Off)]

    private void OnTriggerEnter(Collider other)
    {
        NetworkObject nob = other.GetComponent<NetworkObject>();
        if (nob != null)
            LoadScene(nob);
    }

    private void LoadScene(NetworkObject nob)
    {
        if (!nob.Owner.IsActive)
            return;

        var conns = SceneManager.SceneConnections.Values.ToArray()[0].ToArray();

        
        SceneLoadData sld = new SceneLoadData(SCENE_NAME);
        NetworkObject[] nobs = new NetworkObject[] { };
        
        sld.MovedNetworkObjects = nobs; // carrying all the clients over
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadConnectionScenes(conns, sld);
    }
}
