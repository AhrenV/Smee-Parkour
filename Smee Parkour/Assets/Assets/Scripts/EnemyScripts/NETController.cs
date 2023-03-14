/// Importing NameSpaces
// Default
using UnityEngine;
// Unity namespace for pathfinding.
using UnityEngine.AI;
// All FishNet namespaces are related to Networking
using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing.Scened;
// Other
using System.Linq;

/// Script Class
public class NETController : NetworkBehaviour
{
    public NavMeshAgent agent; // Component used to allow the game object to use Unity's pathfinding methods.
    [SerializeField] ParticleSystem slashFX; // Visual effects for the enemy slash.

    private Transform target; // The position/rotation of the target.
    private float lookSpeed = 10f; // Smoothening speed of the AI, to prevent snappy behaviour.

    // Other variables for calculating AI transform
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Function updates every frame
    void Update()
    {
        bool verified = true;
        if (!target) // Check if the AI doesn't already have a target
        {
            NetworkConnection[] players = new NetworkConnection[] { };
            try
            {
                players = SceneManager.SceneConnections[SceneManager.GetScene("Peaceful")].ToArray(); // Get all the current NetworkConnections which are in the same scene as the AI (in an array)
            }
            catch
            {
                verified = false;
            }
            
            if (verified) // If a valid player is found
            {
                foreach (NetworkConnection conn in players) // Loop through the array.
                {
                    target = conn.FirstObject.transform; // Set the target to the transform of the player
                }
            }
        }

        if (verified) // If a valid player is found
        {
            agent.SetDestination(target.position); // Using the PathFinding method, this finds the shortest way from the AIs current position to the target position, considering its on a baked NavMeshSurface.

            if (agent.remainingDistance <= agent.stoppingDistance) // Check if the AI is close enough to the player to begin attacking, and to stop moving.
            {
                // Calculating parameters for the Lerp method allowing the smoothening of the AIs camera once it gets in range
                _direction = (target.position - transform.position).normalized;
                _lookRotation = Quaternion.LookRotation(_direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, Time.deltaTime * lookSpeed); // This method performs the smoothening, using the current rotation of the AI and calculating a way to smoothly translate it to the target rotation
                if (slashFX.isPlaying) { return; } // If the VFX is already playing the return the function.
                slashFX.Play(); // Play the VFX
            }
        }
        
    }
}