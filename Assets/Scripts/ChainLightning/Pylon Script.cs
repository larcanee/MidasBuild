using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PylonScript : MonoBehaviour
{
    public GameObject aoe_field;
    public bool aoe_active;            //debugging tool & condition check
    public float activation_cooldown;  //cooldown between activation from time end
    public float activation_time;      //time of effect (minimum 1 frame; 1 frame future use case for explosions)
    float last_activation_time;        //time of last effect (must be set on last activation)

    public int uses_remaining = 3;

    public Attack_DroneScript counter;

    public List<GameObject> drones;
    void Start()
    {
        last_activation_time = -activation_cooldown - 1f;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        //deactivation checks
        if (Time.time > last_activation_time + activation_time && aoe_active)
        {
            Debug.Log("Deactivate");
            aoe_field.SetActive(false);
            aoe_active = false;
            uses_remaining--;
            if (uses_remaining <= 0)
            {
                drones.Remove(gameObject);
                Destroy(gameObject);
            }
            last_activation_time = Time.time;
        }
    }

    public void ActivateEffect(string name)
    {
        if (!aoe_active && Time.time > last_activation_time + activation_cooldown)
        {
            Debug.Log("hit " + gameObject.name + " by " + name);
            aoe_field.SetActive(true);
            aoe_active = true;
            last_activation_time = Time.time;
        }
    }
}
