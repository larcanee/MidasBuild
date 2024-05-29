/**
AUTHOR: Dillon Evans

DESCRIPTION:
Tracks and regenerates health and shields.

HOW TO USE:
1. Attach the Health component to the player or an NPC.
2. Adjust the settings of the Health component in the Unity editor.
3. Call Damage() and/or Kill() in script whenever damage should be done.
4. Add listeners to onDeath to call functions when the player/NPC dies.
**/


using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class Health : MonoBehaviour
{
    [Header("Health")]
    [Tooltip("Maximum health.")]
    [Min(0)]
    public int maxHealth = 100;
    [Tooltip("Whether or not health will regenerate.")]
    public bool healthRegeneration = false;


    [Header("Shield")]
    [Tooltip("Whether or not there is a shield.")]
    public bool hasShield = true;
    [Tooltip("Maximum shield.")]
    [Min(0)]
    public int maxShield = 100;
    [Tooltip("Whether or not shield will regenerate.")]
    public bool shieldRegeneration = true;


    [Header("Regeneration")]
    [Tooltip("How long (in seconds) after taking damage before health/shield will begin to regenerate.")]
    [Min(0f)]
    public float regenerationDelay = 2f;
    [Tooltip("How quickly (in units per second) health/shield will regenerate.")]
    [Min(0f)]
    public float regenerationRate = 20f;


    [Header("Death")]
    [Tooltip("Whether or not to destroy this object when health reaches zero.")]
    public bool destroyOnDeath = true;
    [Tooltip("Invoked when health reaches zero.")]
    public UnityEvent<GameObject> onDeath = new();


    [HideInInspector]
    public int health;
    [HideInInspector]
    public int shield;
    [HideInInspector]
    public bool isRegenerating = false;


    private float timeSinceRegenerated = 0f;
    private Coroutine delayRegeneration = null;
    public Animator enemyAnimator;
    
    public EnemyCounter enemyCounter;
    public GameObject Player_Animation; //only exists in the context of the player

    // Damages the player or NPC.
    // `Damage(damage)` should be used instead of `health -= damage`.
    // Returns true if the player/NPC is still alive.
    // Returns false if the player/NPC died.
    public bool Damage(int damage)
    {
        if (hasShield)
        {
            int shieldDamage = Mathf.Min(shield, damage);
            shield -= shieldDamage;
            damage -= shieldDamage;
        }
        health -= damage;
        if (health <= 0)
        {
            Kill();
            if (gameObject.CompareTag("Enemy"))
            {
                //StartCoroutine(EnemyDeathAnimator());
                Debug.Log("Enemy Died");
                enemyCounter.UpdateCount();
            }
            return false;
        }
        StartDelayingRegeneration();
        return true;
    }

    // Damages the player or NPC over time.
    // timeSinceLastDamaged should be initialized to a negative value,
    //   and will then be managed by this function.
    // Returns true if the player/NPC is still alive.
    // Returns false if the player/NPC died.
    public bool Damage(float damagePerSecond, ref float timeSinceLastDamaged)
    {
        int damage;
        if (timeSinceLastDamaged < 0f)
        {
            timeSinceLastDamaged = 0f;
            damage = 1;
        }
        else
        {
            timeSinceLastDamaged += Time.deltaTime;
            damage = Mathf.FloorToInt(damagePerSecond * timeSinceLastDamaged);
            timeSinceLastDamaged -= damage / damagePerSecond;
        }
        return Damage(damage);
    }

    // Kills the player or NPC.
    // `Kill()` should be used instead of `Destroy(gameObject)`.
    public void Kill()
    {
        health = 0;
        onDeath.Invoke(gameObject);
        if(destroyOnDeath)
        {
            if (gameObject.layer == 9)
            {
                Destroy(Player_Animation);
            }
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        health = maxHealth;
        shield = maxShield;
    }

    private void Update()
    {
        if (!isRegenerating) return;
        // Determine the amount to regenerate this frame.
        timeSinceRegenerated += Time.deltaTime;
        int regenerationAmount = Mathf.FloorToInt(timeSinceRegenerated * regenerationRate);
        timeSinceRegenerated -= regenerationAmount / regenerationRate;
        // Regenerate health.
        if (healthRegeneration && health < maxHealth)
        {
            int healthRegenerationAmount = Mathf.Min(maxHealth - health, regenerationAmount);
            health += healthRegenerationAmount;
            regenerationAmount -= healthRegenerationAmount;
        }
        // Regenerate shield.
        if (hasShield && shieldRegeneration && shield < maxShield)
        {
            int shieldRegenerationAmount = Mathf.Min(maxShield - shield, regenerationAmount);
            shield += shieldRegenerationAmount;
            regenerationAmount -= shieldRegenerationAmount;
        }
        // Stop regenerating if at full health/shield.
        if (regenerationAmount > 0)
        {
            isRegenerating = false;
        }
    }


    private IEnumerator DelayRegeneration()
    {
        isRegenerating = false;
        yield return new WaitForSeconds(regenerationDelay);
        StopDelayingRegeneration();
        isRegenerating = true;
    }

    private void StartDelayingRegeneration()
    {
        StopDelayingRegeneration();
        delayRegeneration = StartCoroutine(DelayRegeneration());
    }

    private void StopDelayingRegeneration()
    {
        if (delayRegeneration != null)
        {
            StopCoroutine(delayRegeneration);
            delayRegeneration = null;
        }
    }
}
