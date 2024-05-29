using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(NavMeshAgent))]
public class RangeBehavior : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float projectileCooldown = 2f;
    public float mediumRange = 15f;
    public LayerMask obstacles;
    private Transform playerTransform;
    private float lastShotTime;
    private Transform firePoint;
    private EnemyController enemyController;
    private NavMeshAgent agent;
    public Animator rangeAnimator;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastShotTime = Time.time;
        firePoint = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is within medium range
        if (CanShoot())
        {
            // Don't move when in range.
            enemyController.enabled = false;
            agent.destination = transform.position;
            // Check if enough time has passed since the last shot
            if (Time.time >= lastShotTime + projectileCooldown)
            {
                rangeAnimator.SetBool("isAttacking", true);
                Shoot();
                lastShotTime = Time.time;
            }
        }
        else
        {
            // Chase the player.
            enemyController.enabled = true;
            rangeAnimator.SetBool("isAttacking", false);
        }
    }

    bool CanShoot()
    {
        if (playerTransform == null) return false;
        if (Vector2.Distance(transform.position, playerTransform.position) > mediumRange) return false;
        Vector2 shootDirection = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(
            firePoint.position,
            shootDirection,
            Mathf.Infinity,
            obstacles
        );
        return hit.collider == null || hit.distance > mediumRange;
    }

    void Shoot()
    {
        // Calculate direction towards the player
        Vector2 shootDirection = (playerTransform.position - transform.position).normalized;

        // Instantiate the projectile at the fire point
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Shoot in the direction of the player
        projectile.GetComponent<Rigidbody2D>().velocity = shootDirection * projectileSpeed;
    }
}
