using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class UINETManager : NetworkBehaviour
{
    public Button serverExample;
    public GameObject listContent;

    public override void OnStartClient()
    {
        base.OnStartClient();

    }

    [ObserversRpc]
    public void UIAddServer(int serverID)
    {
        print("UIAddServer");
        Button serverFrame = Instantiate(serverExample);
        serverFrame.transform.SetParent(listContent.transform, false);
        serverFrame.gameObject.SetActive(true);
        serverExample.GetComponent<UINETServerList>().serverID = serverID;
    }
}
