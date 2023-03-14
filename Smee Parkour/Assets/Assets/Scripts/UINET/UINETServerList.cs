using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using System;
using TMPro;

/// Script Class
public class UINETServerList : NetworkBehaviour
{
    public int serverID; // The server ID identifier, allowing clients to know which server they are attempting to join.
    public UINETServers serverManager; // The main script for methods involving networking.
    [SerializeField] TextMeshProUGUI playerCountUI; // Reference to the text of the player count.
    public override void OnStartClient()
    {
        base.OnStartClient();
    }
    // Updates every frame.
    private void Update()
    {
        NetworkConnection[] server = new NetworkConnection[] { }; // Empty list
        bool verified = true; // If the server ID isn't valid then verified = false
        try
        {
            server = serverManager.GetLocalServer(serverID);
        }
        catch (Exception e) // If an error is caused in the try statement the following code will be executed.
        {
            verified = false;
        }
        // If client is connected to a server
        if (verified) // If verified or in other words the server has been found via its ID
        {
            playerCountUI.text = server.Length + "/4"; // Update the text showing the number of players currently in that party.
        }
    }
}
