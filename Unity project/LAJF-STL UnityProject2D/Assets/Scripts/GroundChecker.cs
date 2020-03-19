using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    P1Controller player;

    void Awake(){
        player = transform.GetComponentInParent<P1Controller>();
    }

    void OnCollisionEnter2D(Collision2D coll){
        
    }

}
