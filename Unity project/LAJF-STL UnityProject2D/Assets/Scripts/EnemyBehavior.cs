using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public Rigidbody2D rb2;
    public GameObject target;
    public Enemy agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.health < 0)
        {
            return;
        }

        MoveTowards(target.transform);
    }

    private void MoveTowards(Transform tf)
    {
        Vector2 direction = tf.position - transform.position;
        rb2.AddRelativeForce((direction).normalized * agent.speed);
    }


}
