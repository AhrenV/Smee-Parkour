using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class UINETManager : NetworkBehaviour
{
    public Button serverExample;
    public UINETServers serverManager;
    public GameObject listContent;

    public override void OnStartClient()
    {
        base.OnStartClient();
        foreach(int i in serverManager.GetLocalServerIDs())
        {
            print("addy");
            UIAddServer(i);
        }
    }

    [ObserversRpc]
    public void UIAddServer(int serverID)
    {
        Button serverFrame = Instantiate(serverExample);
        serverFrame.transform.SetParent(listContent.transform, false);
        serverFrame.gameObject.SetActive(true);
        serverFrame.GetComponent<UINETServerList>().serverID = serverID;
    }
}
