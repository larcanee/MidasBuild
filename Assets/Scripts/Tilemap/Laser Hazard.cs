using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHazard : MonoBehaviour
{
    public float active_time = 2f;
    public float last_active_time;
    public float cooldown_time = 3;
    public bool on = true;
    // Start is called before the first frame update
    void Start()
    {
        last_active_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!on && Time.time > last_active_time + cooldown_time) 
        {
            last_active_time = Time.time;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            on = true;
        }
        else if (on && Time.time > last_active_time + active_time)
        {
            last_active_time = Time.time;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            on = false;
        }
    }
}
