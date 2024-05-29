using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public string playerTag = "Player";
    public EnemyVisualBehavior visualBehavior;


    private NavMeshAgent navMeshAgent = null;
    private GameObject player = null;


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(playerTag);
        // Stop the agent from rotating to face the player,
        // which would cause them to not be visible to the camera.
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Update()
    {
        if (player)
        {
            navMeshAgent.destination = player.transform.position;
        }

        // Face the direction the agent is moving.
        if (navMeshAgent.velocity.x > 0)
        {
            visualBehavior.FaceRight();
        }
        else if (navMeshAgent.velocity.x < 0)
        {
            visualBehavior.FaceLeft();
        }
    }
}
