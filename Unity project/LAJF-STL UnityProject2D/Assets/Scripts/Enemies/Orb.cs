using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int health;

    public LayerMask layerMask; //Layers this object should interact with and damage.

    public GameObject particleExplosionObj;

    [Range(1, 30)]
    public int damage;

    private OrbEnemy parentOrb;

    [SerializeField]
    [Range(0.5f, 10f)]
    private float projectileSpeed;

    public void Start()
    {
        parentOrb = transform.parent.GetComponent<OrbEnemy>();
    }


    public void TakeDamage(int dmg)
    {
        health -= dmg;

        //TODO: Add some blinking effect or something.

        if (health <= 0)
        {
            Debug.Log("Orb died");
            Instantiate(particleExplosionObj);
            transform.parent.GetComponent<OrbEnemy>().isOrbDead = true;
            Destroy(gameObject);
            //TODO: Add death explosion, sounds and particles and stuff.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (Projectile.IsInLayerMask(other.layer, layerMask))
        {
            //Visuals, sounds and other could be here, rather than in "takedamage". 
        }
    }

    public void Attack(Transform target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(parentOrb.bulletObj, transform.position + (((Vector3)direction) * 0.2f), Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        bullet.GetComponent<Projectile>().damage = damage;

    }
}
