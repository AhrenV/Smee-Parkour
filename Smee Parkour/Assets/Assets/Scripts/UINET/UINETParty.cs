/// Importing NameSpaces
// Default
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// All FishNet namespaces are related to Networking
using FishNet.Object;
using FishNet.Connection;
// Other
using System;

/// NOTE: This is a stub of some sorts, and was created with the intention for testing, thus efficiency was not considered during the programming of this class.

/// Script Class
public class UINETParty : NetworkBehaviour
{
    public GameObject PFP; // A prefab for the player indicator on the bottom left (to identify how many players are in a party together).
    public UINETServers serverManager; // Gets the main script (UINETServers) holds all the important methods/subroutines involving networking the main menu.
    [SerializeField] int memberInt; // An identifier of which member slot the script should keep track of.
    NETClientSettings localSettings; // The settings of the local client (allows the script to get information such as their ServerID)

    // This function runs when this object becomes first visible to the client.
    public override void OnStartClient()
    {
        base.OnStartClient();
        localSettings = base.LocalConnection.FirstObject.GetComponent<NETClientSettings>(); // Get the local player's client settings.
    }

    // This function loops once per frame.
    private void Update()
    {
        NetworkConnection[] server = new NetworkConnection[] { }; // Create an empty list.
        bool verified = true; // Used to check if the player is currently in a server/party (used as a debounce of some sorts)

        try // Try statement prevents errors from crashing anything, it will run the catch part if an error needs to be handled.
        {
            server = serverManager.GetLocalServer(localSettings.ServerID); // Get the list of NetworkConnections of the local player's current party.
        }
        catch
        {
            verified = false; // Set verified to false as no server has been found, or some other error.
        }
        
        if (verified) // If verified
        {
            if (server.Length - 1 >= memberInt) // If the party has an nth member, depending on the memberInt variable.
            {
                PFP.SetActive(true); // Set the indicator to active, allowing the players in the party to see that a new player has joined them.
            }
        }
    }

}
