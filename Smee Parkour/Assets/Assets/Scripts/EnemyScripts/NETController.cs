using UnityEngine;
using UnityEngine.AI;
using FishNet.Object;
using FishNet.Connection;
using System.Linq;
using FishNet.Managing.Scened;

public class NETController : NetworkBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] ParticleSystem slashFX;

    private Transform target;
    private float lookSpeed = 10f;
    private Quaternion _lookRotation;
    private Vector3 _direction;

    void Update()
    {
        if (!target) {
            print("not found");
            NetworkConnection[] players = SceneManager.SceneConnections[SceneManager.GetScene("Network Testing")].ToArray();
            
            foreach (NetworkConnection conn in players)
            {
                target = conn.FirstObject.transform;
            }
        }

        agent.SetDestination(target.position);
        
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            _direction = (target.position - transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, Time.deltaTime * lookSpeed);
            if (slashFX.isPlaying) { return; }
            slashFX.Play();
        }
    }
}