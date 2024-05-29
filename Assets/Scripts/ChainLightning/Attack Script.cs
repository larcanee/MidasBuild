using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackScript : MonoBehaviour
{
    public GameObject lightning_bolt;
    public GameObject lightning_aoe;
    public Animator midasAnimator;
    //public float attack_cooldown;  //cooldown between attacks
    //public float attack_time;      //time of effect for attack (minimum 1 frame)
    //float last_attack_time; //time of last attack (must be set by attack)
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // RotationMovement();
        //if (//(Time.time > last_attack_time + attack_cooldown)
        //{
        if (Input.GetKey(KeyCode.Z) || Input.GetMouseButton(0))
        {
            lightning_bolt.SetActive(true);
            lightning_aoe.SetActive(false);
            midasAnimator.SetBool("isAttacking", true);
            //last_attack_time = Time.time;
        }
        else if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(1))
        {
            lightning_aoe.SetActive(true);
            lightning_bolt.SetActive(false);
            midasAnimator.SetBool("isAttacking", true);
            //last_attack_time = Time.time;
        }
        else
        {
            lightning_bolt.SetActive(false);
            lightning_aoe.SetActive(false);
            midasAnimator.SetBool("isAttacking", false);
        }
        //}
        /*
        else if (Time.time > last_attack_time + attack_time)
        {
            lightning_bolt.SetActive(false);
            lightning_aoe.SetActive(false);
        }*/

    }
}
