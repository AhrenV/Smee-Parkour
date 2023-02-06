using FishNet;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet.Managing.Logging;

public class SceneTesting : NetworkBehaviour
{
    public const string SCENE_NAME = "Test Realm";

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

        SceneLoadData sld = new SceneLoadData(SCENE_NAME);
        sld.MovedNetworkObjects = new NetworkObject[] { nob };
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadConnectionScenes(nob.Owner, sld);
    }
}
