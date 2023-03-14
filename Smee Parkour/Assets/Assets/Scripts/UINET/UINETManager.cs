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

    // This function runs when this object becomes first visible to the client.
    public override void OnStartClient()
    {
        base.OnStartClient();
        // Gets all already existing servers and adds buttons for them to the server list, allowing avaialble parties to be joinable by other players.
        foreach(int i in serverManager.GetLocalServerIDs())
        {
            UIAddServer(i); // Call the local function within this class which adds the new button to the server list.
        }
    }

    // Function to add a server banner/button indicator to the server list to allow players to join
    [ObserversRpc]
    public void UIAddServer(int serverID)
    {
        Button serverFrame = Instantiate(serverExample); // Duplicate the prefab.
        serverFrame.transform.SetParent(listContent.transform, false); // Set the parent to the List Content object.
        serverFrame.gameObject.SetActive(true); // Make the object visisble
        serverFrame.GetComponent<UINETServerList>().serverID = serverID; // Set the current serverID of this button to the one it correlates to, is an indicator of what server/party the button represents.
    }
}
