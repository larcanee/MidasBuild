using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FaceRight()
    {
        GetComponent<SpriteRenderer>().flipX = false;
    }

    public void FaceLeft()
    {
        GetComponent<SpriteRenderer>().flipX = true;
    }
}
