using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public int damage = 3;
    bool destroyed;
    bool active = false;
    float explosion_length = 0.5f;
    float explosion_time;

    List<GameObject> objects_in_radius = new List<GameObject>();

    float exposure_limit = 1;
    float exposure_time = 0;
    private void Update()
    {
        if (!active && objects_in_radius.Count > 0)
        {
            active = true;
            exposure_time = Time.time;
        }
        else if (active && Time.time > exposure_time + exposure_limit)
        {
            Debug.Log("landmine Trigger");
            Explode();
        }
        if (destroyed && Time.time > explosion_length + explosion_time)
        {
            Destroy(gameObject);
        }
    }

    //Needs revision for final version (based on continuous exposure and blast all within)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            objects_in_radius.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            objects_in_radius.Remove(collision.gameObject);
        }
        if (objects_in_radius.Count == 0)
        {
            active = false;
        }
    }

    public void Explode()
    {
        foreach (GameObject obj in objects_in_radius)
        {
            if (obj.TryGetComponent(out Health health))
            {
                health.Damage(damage);
            }
            else
            {
                Debug.LogWarning("Landmine detected something it shouldn't");
            }
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        destroyed = true;
        explosion_time = Time.time;
    }
}
