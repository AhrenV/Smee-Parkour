/// Importing NameSpaces
// Default
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// All FishNet namespaces are related to Networking
using FishNet.Object;

/// NOTE: The term function and subroutine will be used interchangably.

/// Script Class
public class PartyCreator : NetworkBehaviour
{
    public UINETServers serverManager; // Gets the main script (UINETServers) holds all the important methods/subroutines involving networking the main menu.

    // OnClick function for connecting a player.
    public void ConnectPlayer(int serverID)
    {
        serverManager.ConnectPlayer(serverID); // Calling the ServerRPC (method which will be called on the server end) to connect the player who called this function.
    }

    // OnClick function for connecting a player.
    public void CreateServer()
    {
        serverManager.CreateServer(); // Calling the ServerRPC to create a new server for the player who called this function.
    }

    // OnClick function for teleporting a party/server to a NEW level.
    public void TeleportServer()
    { 
        serverManager.LoadLobby("Peaceful", 0); // Calling the ServerRPC to load a specific party/server into a new level/scene.
    }

}
