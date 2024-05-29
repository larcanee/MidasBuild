using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class PylonNav : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform; // Reference to the player's transform
    public float moveSpeed = 2f; // Speed at which the enemy moves towards the player
    public bool target_player = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();


        // Find the player GameObject and get its transform
        if (target_player)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            playerTransform = GameObject.FindGameObjectWithTag("Reticle").transform;
        }

        // Stop the agent from rotating to face the player,
        // which would cause them to not be visible to the camera.
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void Update()
    {
        // Move towards the player
        if (playerTransform != null)
        {
            // Vector3 direction = (playerTransform.position - transform.position).normalized;
            // transform.position += direction * moveSpeed * Time.deltaTime;
            agent.destination = playerTransform.position;
        }
    }
}
