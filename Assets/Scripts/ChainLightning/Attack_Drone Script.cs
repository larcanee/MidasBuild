using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(Mana))]
public class Attack_DroneScript : MonoBehaviour
{
    public float spawn_cooldown;  //cooldown between attacks
    float last_spawn_time; //time of last attack (must be set by attack)
    public int drone_limit;
    public int drone_mana;
    public List<GameObject> drones = new List<GameObject>();
    public GameObject drone_prefab;

    int count;
    public TMP_Text droneText;

    private Mana mana = null;


    private void Start()
    {
        mana = GetComponent<Mana>();
        // Allow the drones to be spawned right away.
        last_spawn_time = -spawn_cooldown;
    }


    private void Update()
    {
        if (Time.time > last_spawn_time + spawn_cooldown)
        {
            if (Input.GetKey(KeyCode.E) && drones.Count < drone_limit && mana.UseMana(drone_mana))
            {
                //create drone
                GameObject newdrone = Instantiate(drone_prefab, transform.position, transform.rotation);
                drones.Add(newdrone);
                newdrone.GetComponent<PylonScript>().drones = drones;
                newdrone.name = "drone" + count;
                last_spawn_time = Time.time;
                IncrementCount();
            }
            if (Input.GetKey(KeyCode.Q) && drones.Count < drone_limit && mana.UseMana(drone_mana))
            {
                //create drone
                GameObject newdrone = Instantiate(drone_prefab, transform.position, transform.rotation);
                drones.Add(newdrone);
                newdrone.GetComponent<PylonScript>().drones = drones;
                newdrone.name = "drone" + count;
                newdrone.GetComponent<PylonNav>().target_player = true;
                last_spawn_time = Time.time;
                IncrementCount();
            }
        }
    }

    public void IncrementCount()
    {
        count++;
        droneText.text = "Drones Deployed: " + count;
    }

    public void DecrementCount()
    {
        count--;
        droneText.text = "Drones Deployed: " + count;
    }
}
