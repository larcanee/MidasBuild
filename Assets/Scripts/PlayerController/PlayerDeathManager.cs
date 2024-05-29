using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class PlayerDeathManager : MonoBehaviour
{
    public List<MonoBehaviour> disableOnDeath = new();
    public List<GameObject> destroyOnDeath = new();


    public void Die()
    {
        foreach (MonoBehaviour component in disableOnDeath)
        {
            component.enabled = false;
        }
        foreach (GameObject gameObject in destroyOnDeath)
        {
            Destroy(gameObject);
        }

    }
}
