/**
AUTHOR: Dillon Evans

DESCRIPTION:
Tracks and regenerates mana.

HOW TO USE:
1. Attach the Mana component to the player or an NPC.
2. Adjust the settings of the Mana component in the Unity editor.
3. Call UseMana() in script whenever mana should be used.
**/


using System.Collections;
using UnityEngine;


public class Mana : MonoBehaviour
{
    [Tooltip("Maximum mana.")]
    [Min(0)]
    public int maxMana = 100;
    [Tooltip(
        "If false, starts with the maximum mana.\n" +
        "If true, starts with the initial mana."
    )]
    public bool useInitialMana = false;
    [Tooltip("Initial mana. Only used if useInitialMana is true.")]
    [Min(0)]
    public int initialMana = 100;
    [Tooltip("Whether or not mana will regenerate.")]
    public bool regeneration = true;
    [Tooltip("How long (in seconds) after using mana before it will begin to regenerate.")]
    [Min(0f)]
    public float regenerationDelay = 2f;
    [Tooltip("How quickly (in units per second) mana will regenerate.")]
    [Min(0f)]
    public float regenerationRate = 20f;


    [HideInInspector]
    public int mana;
    [HideInInspector]
    public bool isRegenerating = false;


    private float timeSinceRegenerated = 0f;
    private Coroutine delayRegeneration = null;


    // Uses the amount of mana.
    // `UseMana(amount)` should be used instead of `mana -= amount`.
    // Returns true if there was enough mana.
    // Returns false if there was not enough mana.
    public bool UseMana(int amount)
    {
        if (mana < amount) return false;
        mana -= amount;
        StartDelayingRegeneration();
        return true;
    }


    private void Start()
    {
        mana = useInitialMana ? initialMana : maxMana;
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
        mana += Mathf.Min(maxMana - mana, regenerationAmount);
        // Stop regenerating if at full mana.
        if (mana >= maxMana)
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
