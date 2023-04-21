/// Importing NameSpaces
// Default
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// All FishNet namespaces are related to Networking
using FishNet.Object;

/// NOTE: This script holds all the LOCAL User Interface related methods.

/// Script Class
public class UINETManager : NetworkBehaviour
{
    public Button serverExample; // Prefab for the button (the element in the server list tab of the game which enables players to join available parties)
    public UINETServers serverManager; // The main script for networking, holds important subroutines.
    public GameObject listContent; // The gameobject which holds all the preabs for allowing players to join new parties.

    private List<int> UIservers = new List<int> { };

    public override void OnStartClient()
    {
        base.OnStartClient();
        InvokeRepeating("CheckServers", 1.0f, 1.0f);
    }

    void CheckServers()
    {
        foreach (int ID in serverManager.GetLocalServerIDs())
        {
            if (!UIservers.Contains(ID)) {
                UIAddServer(ID);
                UIservers.Add(ID);
            }
        }
    }

    // Function to add a server banner/button indicator to the server list to allow players to join
    public void UIAddServer(int serverID)
    {
        print("Add server: " + serverID);
        Button serverFrame = Instantiate(serverExample); // Duplicate the prefab.
        serverFrame.transform.SetParent(listContent.transform, false); // Set the parent to the List Content object.
        serverFrame.gameObject.SetActive(true); // Make the object visisble
        serverFrame.GetComponent<UINETServerList>().serverID = serverID; // Set the current serverID of this button to the one it correlates to, is an indicator of what server/party the button represents.
    }
}
