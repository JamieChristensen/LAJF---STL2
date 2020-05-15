using System;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public static int projectileCount = 0;
    public int damage;
    public LayerMask layer;




    // Start is called before the first frame update
    void Start()
    {
        projectileCount++;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collided = collision.gameObject;
        Destroy(gameObject);

        //if (Projectile.IsInLayerMask(collision.gameObject.layer, layer))
        //    Debug.Log("Fireball hit!");
        if (collided.CompareTag("Player"))
        {
            collided.GetComponent<P1Controller>().TakeDamage(damage);
        }

    }
}

