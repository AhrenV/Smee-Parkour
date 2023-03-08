using FishNet;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet.Managing.Logging;
using FishNet.Component.Observing;

public class SceneLoader : NetworkBehaviour
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

        SceneLookupData lookup = new SceneLookupData(_stackedSceneHandle, SCENE_NAME);
        SceneLoadData sld = new SceneLoadData(lookup);
        sld.Options.AllowStacking = true;

        sld.MovedNetworkObjects = new NetworkObject[] { nob };
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadConnectionScenes(nob.Owner, sld);

        MatchCondition.AddToMatch(1, nob.Owner);
    }

    public bool SceneStack = false;
    private int _stackedSceneHandle = 0;

    private void Start()
    {
        InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
    }

    private void OnDestroy()
    {
        if (InstanceFinder.SceneManager != null)
            InstanceFinder.SceneManager.OnLoadEnd -= SceneManager_OnLoadEnd;
    }

    private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs obj)
    {
        if (!obj.QueueData.AsServer)
            return;
        if (!SceneStack)
            return;
        if (_stackedSceneHandle != 0)
            return;

        if (obj.LoadedScenes.Length < 0)
            _stackedSceneHandle = obj.LoadedScenes[0].handle;
    }

}
