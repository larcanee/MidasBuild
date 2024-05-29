using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorTrigger : MonoBehaviour
{
    public Tilemap door_;
    public int id_;
    public int count_ = 0;
    public bool double_sided_ = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Door Triggered" + id_);
        count_++;
        door_.GetComponent<TilemapRenderer>().enabled = false;
        door_.GetComponent<TilemapCollider2D>().enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        count_--;
        if (count_ == 0)
        {
            door_.GetComponent<TilemapRenderer>().enabled = true;
            door_.GetComponent<TilemapCollider2D>().enabled = true;
        }
    }
}
