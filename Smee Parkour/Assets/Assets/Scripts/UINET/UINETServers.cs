/// Importing NameSpaces
// Default
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
// All FishNet namespaces are related to Networking
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using FishNet.Component.Observing;
using FishNet.Managing.Scened;
// Other
using System.Linq;

/// SCRIPT CLASS
public class UINETServers : NetworkBehaviour
{
    // GLOBAL VARIABLES
    public UINETManager UIManager; // Reference to the UI Manager script which holds functions/subroutines for User Interface based procedures.
    private readonly char delimiter = '-'; // The connector used for processing ServerStrings.
    private readonly char mapDelimiter = '/'; // Used for separating client data from map data.
    private readonly int partySize = 4; // Maximum number of players which can join a party/lobby/server.
    private readonly string demoScene = "Peaceful"; // Name of the DEMO scene.
    
    // ** PARTY DATABASE **
    [SyncObject]
    private readonly SyncList<string> SERVERS = new SyncList<string>() {};

    /// NOTE: The reason I didn't use a 2D List/Datastructure (E.g. A List of HashSets) is because FishNet does not support the serialization of 2D data structures NATIVELY, so if I were to use 2D data structurs I would need to make a custom serialiser, which is rather complex and I prefer to use this approach for that sake.
    // If this were NOT the case, then I would most certainly have used a 2D data structure, E.g. List of HashSets, or a List of Lists.

    // -------- Allow SyncList to inherit the same behaviour as a Regular List would. (Not very important to understand, just documentation code)
    private void Awake()
    {
        SERVERS.OnChange += _myCollection_OnChange; // Subscribing to the OnChange event.
    }

    private void _myCollection_OnChange(SyncListOperation op, int index,
         string oldItem, string newItem, bool asServer)
    {
        switch (op) // A sequence of operations which can be performed on the list.
        {
            case SyncListOperation.Add:
                break;

            case SyncListOperation.RemoveAt:
                break;

            case SyncListOperation.Insert:
                break;

            case SyncListOperation.Set:
                break;

            case SyncListOperation.Clear:
                break;

            case SyncListOperation.Complete:
                break;
        }
    }
    /// ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    /// NOTE: The terms "Server" and "Party" may be used interchangably. To avoid any confusion, think of these terms as a group of players who have now banded together, who will play a specific level together.

    /// --------------------------------------------------------------------------------------------ALL SERVER SIDE FUNCTIONS / SUBROUTINES----------------------------------------------------------------------------------------------------------------



    /// FUNCTION / SUBROUTINE to Create a Server
    [ServerRpc(RequireOwnership = false)] // Allows clients to call the RPC regardless if they are the "Network Owner" of the object.
    public void CreateServer(string SELECTED_LEVEL, NetworkConnection conn = null) // conn is a default parameter passed when calling a ServerRPC (Remote procedure call), which is an object that allows the programmer to access certain properties of the Client (such as their Client ID)
    {
        NETClientSettings settings = conn.FirstObject.GetComponent<NETClientSettings>(); // Get the client's "Settings", which are stored in the Player Character

        if (settings.ServerID != -1) { print("You can't make more than 1 server");  return;  } // Prevent players from joining a different party if they are already in one (-1 is the default value for clients who haven't joined a party yet)

        string new_server = SELECTED_LEVEL+mapDelimiter+"-"+conn.ClientId+"-"; // Create a new Server String to add to the **Party Database**, with the players' Client ID and selected map (Client ID used as identifier to recognise certain players)
        SERVERS.Add(new_server); // Add the Server string to the **Party Database**

        int serverID = SERVERS.IndexOf(new_server); // This gets the unique server ID of the Server (Each server has their own Unique Identifier, being their index within the Database)

        conn.FirstObject.GetComponent<NETClientSettings>().ServerID = serverID; // Setting the ServerID (in our understanding the ID of the party the player is in) to the new value (as the player has just "joined" a new server)

        //UIManager.UIAddServer(serverID); // This function/method adds a new button to the server list, allowing it be seen by new players, allowing them to join the server/party too.

        print("Server created with ID: " + serverID); // Debug purposes, just prints out the Server's ID that was just created.
    }



    /// FUNCTION / SUBROUTINE to Connect a Player to a new Server
    [ServerRpc(RequireOwnership = false)]
    public void ConnectPlayer(int serverID, NetworkConnection conn = null) // The serverID is passed through by the client, allowing the function to specify which server/party the player wants to join.
    {
        NETClientSettings settings = conn.FirstObject.GetComponent<NETClientSettings>(); // Obtain client settings

        if (settings.ServerID != -1) { print("You can't join a new server when you're currently in one"); return; } // Check whether the player/client is already in a server, if they are then return the function (ending it).

        // Adding player to SERVERS datatable
        List<NetworkConnection> server = DecodeString(SERVERS[serverID]); // Access the ServerString using the unique serverID passed through from the **Party Database**, converting it into a list allowing us to access all the clients properly.
        string MAP = DecodeMap(SERVERS[serverID]);
        
        if (server.Count >= partySize || server.Contains(conn)) // Check if the server is full, or the player/client is already in the party. If they are then return the function
        { print("party full or already joined");  return; }

        server.Add(conn); // Add the player/client to the Server String.

        conn.FirstObject.GetComponent<NETClientSettings>().ServerID = serverID; // Sync the Client Settings, updating the ServerID they are currently in, since they joined a new party.

        SERVERS[serverID] = EncodeString(server, MAP); // Overwrite the old ServerString with the new one which INCLUDES the new player
    }
    


    /// FUNCTION / SUBROUTINE to Load a Party/Server into a NEW Level
    [ServerRpc(RequireOwnership = false)]
    public void LoadLobby(NetworkConnection conn = null) // SCENE_NAME is the name of the scene the party will load in to. ServerID, is the ID of the Server/Party that is to be loaded into a new Scene/Level
    {
        int serverID = conn.FirstObject.GetComponent<NETClientSettings>().ServerID;

        NetworkConnection[] conns = DecodeString(SERVERS[serverID]).ToArray(); // Grabbing the list of players in the current server/party.
        string SCENE_NAME = DecodeMap(SERVERS[serverID]);

        print("Loading Server ID: " + serverID + " into " + SCENE_NAME);

        // Make SLD object
        SceneLoadData sld = new SceneLoadData(SCENE_NAME); // Make an SLD object, essentially allows me to specify objects that will load in to the new level, and the players who will be teleported along with it.

        List<NetworkObject> nobs = new List<NetworkObject> { }; // Create a list of objects which will be passed through.

        // Looping through all of the players in the current Party
        foreach (NetworkConnection con in conns)
        {
            NetworkObject obj = con.FirstObject.GetComponent<NetworkObject>(); // Retrieving the NetworkObject component of the player's character (this is what is needed to pass the object into the new scene)

            nobs.Add(obj); // Adding this object into the empty list created previously.
        }

        // Add Nobs to moved + convert to array
        sld.MovedNetworkObjects = nobs.ToArray(); // Add the list of Objects into the SLD object, converting it to an array (SLD supports arrays, not lists)

        sld.ReplaceScenes = ReplaceOption.All; // Useful to prevent empty scenes which will take up unnessesary strain on the server.

        SceneManager.LoadConnectionScenes(conns, sld); // Load the all the clients/players into the new Level alongside their player characters.

        MatchCondition.AddToMatch(serverID, conns); // Add to a specific Match, with the serverID being the identifier, and the conns being the list of players (this feature is to prevent clients to see eachother from different levels)
    }



    /// FUNCTION / SUBROUTINE to Decode a Server String (into a List of NetworkConnections) <--- COMPLEX ALGORITHM
    private List<NetworkConnection> DecodeString(string serverString)
    {
        string[] IDS = (serverString.Split('/')[1]).Split(delimiter); // Splits the string into an array using the specified delimiter, E.g. "0-3" becomes {0, 3}

        IDS = IDS.Where(x => !string.IsNullOrEmpty(x)).ToArray(); // Removes all the empty entries from the array.

        List<NetworkConnection> nobs = new List<NetworkConnection> { }; // Creating a new empty List of type NetworkConnection (allows us to access players' information)

        // Iterate through the array of ClientID integers
        foreach (string ID in IDS) 
        {
            nobs.Add(ServerManager.Clients[int.Parse(ID)]); // Add the NetworkConnection object of the client to the empty List through the use of the Players ClientID to obtain said Object.
        }
        return nobs; // Returning a list of NetworkObjects E.g, {NetworkConnection1, NetworkConnection2}
    }

    /// FUNCTION / SUBROUTINE to Decode the Map from a Server String.
    private string DecodeMap(string serverString)
    {
        return serverString.Split('/')[0];
    }



    /// FUNCTION / SUBROUTINE to Encode a Server String (into a ServerString)
    private string EncodeString(List<NetworkConnection> conns, string mapName) // List of NetworkConnections to Encode
    {
        List<int> IDs = new List<int> { }; // Declare a new empty list of integers

        // Loop through the list passed as a parameter
        foreach (NetworkConnection conn in conns)
        {
            IDs.Add(conn.ClientId); // Add the ClientID of the player into the empty list of integers declared previously.
        }

        return mapName + mapDelimiter + delimiter + string.Join(delimiter, IDs) + delimiter; // Return a newly formatted server string, E.g. "Peaceful/-0-3-"
    }


    /// --------------------------------------------------------------------------------------------ALL SERVER SIDE FUNCTIONS / SUBROUTINES----------------------------------------------------------------------------------------------------------------


    /// FUNCTION / SUBROUTINE to get a list of IDs from ALL the active Server/Party's
    public int[] GetLocalServerIDs()
    {
        return SERVERS.Select(x => SERVERS.IndexOf(x)).ToArray(); // More complex way of a for loop and getting the index of the element within the list, then converting to an array.
        /// Explanation:
        // We name the element that is currently being iterated over as x, we get the index of x within the **Party Database** data structure, and that will now be part of the new list.
        // You can find more information on this in the LINQ documentation (uses the namespace System.Linq)
    }

    /// FUNCTION / SUBROUTINE to get the server locally
    public NetworkConnection[] GetLocalServer(int serverID) // Parameter being the specified ServerID to get the player list for.
    {
        return DecodeLocalString(SERVERS[serverID]).ToArray(); // Return the decoded string, and convert the datatype from a list to an array
    }

    /// FUNCTION / SUBROUTINE to Decode a ServerString locally (You need to use a different method to obtain a player's NetworkObject locally compared to from the server due to security reasons).
    private List<NetworkConnection> DecodeLocalString(string serverString)
    {
        string[] IDS = (serverString.Split('/')[1]).Split(delimiter); // Splits the string into an array using the specified delimiter, E.g. "0-3" becomes {0, 3}

        IDS = IDS.Where(x => !string.IsNullOrEmpty(x)).ToArray(); // Removes all the empty entries from the array.

        List<NetworkConnection> nobs = new List<NetworkConnection> { }; // Creating a new empty List of type NetworkConnection (allows us to access players' information)

        // Iterate through the array of ClientID integers
        foreach (string ID in IDS)
        {
            nobs.Add(ClientManager.Clients[int.Parse(ID)]); // Add the NetworkConnection object of the client to the empty List through the use of the Players ClientID to obtain said Object.
        }
        return nobs; // Returning a list of NetworkObjects E.g, {NetworkConnection1, NetworkConnection2}
    }

    private void Update()
    {
        foreach (var server in SERVERS)
        {
            print(SERVERS.IndexOf(server) + ": " + server);
        }
    }

}

/// --------------------------------------------------------------------------------------------END OF SCRIPT------------------------------------------------------------------------------------------------------------------------------------------------