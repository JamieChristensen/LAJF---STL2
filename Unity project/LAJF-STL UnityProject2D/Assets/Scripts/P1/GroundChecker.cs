using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    P1Controller player;

    void Awake()
    {
        player = transform.GetComponentInParent<P1Controller>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("triggered");
        if (coll.CompareTag("Ground")){
            player.isGrounded = true;
            player.audioList.PlayWithVariablePitch(player.audioList.land);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Ground")){
            player.isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Ground")){
            player.isGrounded = false;
        }
    }

}
