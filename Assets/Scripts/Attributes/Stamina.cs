/**
AUTHOR: Dillon Evans

DESCRIPTION:
Tracks and regenerates stamina.

HOW TO USE:
1. Attach the Stamina component to the player or an NPC.
2. Adjust the settings of the Stamina component in the Unity editor.
3. Call UseStamina() in script whenever stamina should be used.
**/


using System.Collections;
using UnityEngine;


public class Stamina : MonoBehaviour
{
    [Tooltip("Maximum stamina.")]
    [Min(0)]
    public int maxStamina = 100;
    [Tooltip(
        "If false, starts with the maximum stamina.\n" +
        "If true, starts with the initial stamina."
    )]
    public bool useInitialStamina = false;
    [Tooltip("Initial stamina. Only used if useInitialStamina is true.")]
    [Min(0)]
    public int initialStamina = 100;
    [Tooltip("Whether or not stamina will regenerate.")]
    public bool regeneration = true;
    [Tooltip("How long (in seconds) after using stamina before it will begin to regenerate.")]
    [Min(0f)]
    public float regenerationDelay = 2f;
    [Tooltip("How quickly (in units per second) stamina will regenerate.")]
    [Min(0f)]
    public float regenerationRate = 20f;


    [HideInInspector]
    public int stamina;
    [HideInInspector]
    public bool isRegenerating = false;


    private float timeSinceRegenerated = 0f;
    private Coroutine delayRegeneration = null;


    // Uses the amount of stamina.
    // `UseStamina(amount)` should be used instead of `stamina -= amount`.
    // Returns true if there was enough stamina.
    // Returns false if there was not enough stamina.
    public bool UseStamina(int amount)
    {
        if (amount == 0) return true;
        if (stamina < amount) return false;
        stamina -= amount;
        StartDelayingRegeneration();
        return true;
    }

    // Uses stamina over time.
    // timeSinceLastUsedStamina should be initialized to a negative value,
    //   and will then be managed by this function.
    // Returns true if there was enough stamina.
    // Returns false if there was not enough stamina.
    public bool UseStamina(float amountPerSecond, ref float timeSinceLastUsedStamina)
    {
        if (stamina <= 0) return false;
        int amount;
        if (timeSinceLastUsedStamina < 0f)
        {
            timeSinceLastUsedStamina = 0f;
            amount = 1;
        }
        else
        {
            timeSinceLastUsedStamina += Time.deltaTime;
            amount = Mathf.FloorToInt(amountPerSecond * timeSinceLastUsedStamina);
            timeSinceLastUsedStamina -= amount / amountPerSecond;
        }
        if (stamina < amount)
        {
            stamina = 0;
            StartDelayingRegeneration();
            return false;
        }
        return UseStamina(amount);
    }

    public void DelayRegeneration()
    {
        StartDelayingRegeneration();
    }


    private void Start()
    {
        stamina = useInitialStamina ? initialStamina : maxStamina;
        StartDelayingRegeneration();
    }

    private void Update()
    {
        if (!isRegenerating) return;
        if (!regeneration)
        {
            isRegenerating = false;
            return;
        }
        // Determine the amount to regenerate this frame.
        timeSinceRegenerated += Time.deltaTime;
        int regenerationAmount = Mathf.FloorToInt(timeSinceRegenerated * regenerationRate);
        timeSinceRegenerated -= regenerationAmount / regenerationRate;
        // Regenerate mana.
        stamina += Mathf.Min(maxStamina - stamina, regenerationAmount);
        // Stop regenerating if at full mana.
        if (stamina >= maxStamina)
        {
            isRegenerating = false;
        }
    }


    private IEnumerator RegenerationDelay()
    {
        isRegenerating = false;
        yield return new WaitForSeconds(regenerationDelay);
        StopDelayingRegeneration();
        isRegenerating = true;
    }

    private void StartDelayingRegeneration()
    {
        StopDelayingRegeneration();
        delayRegeneration = StartCoroutine(RegenerationDelay());
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
