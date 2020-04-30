using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int health;

    public LayerMask layerMask; //Layers this object should interact with and damage.

    public GameObject particleExplosionObj;


    public void TakeDamage(int dmg)
    {
        health -= dmg;

        //TODO: Add some blinking effect or something.

        if (health <= 0)
        {
            Debug.Log("Orb died");
            //TODO: Add death explosion, sounds and particles and stuff.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (Projectile.IsInLayerMask(other.layer, layerMask))
        {
            
        }
    }
}
