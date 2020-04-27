using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public LayerMask layerMask; //Layers this object should interact with and damage.

    public ParticleSystem particles;
    private float timer = 0;
    private float maxTime = 0.1f;

    [SerializeField]
    private float explosionRadius = 10;

    public bool isExplodingProjectile = false;

    [SerializeField]
    private GameObject particleExplosion;





    public void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject collided = coll.gameObject;
        if (IsInLayerMask(collided.layer, layerMask))
        {
            if (isExplodingProjectile)
            {
                Explode();
                return;
                //If the projectile explodes, it will always do damage to the object it collided with too when it explodes.
            }

            if (collided.CompareTag("Monster"))
            {
                //My best guess of what Luca would implement:
                collided.GetComponent<EnemyBehaviour>().TakeDamage(damage);
            }

            if (collided.CompareTag("Player"))
            {
                collided.GetComponent<P1Controller>().TakeDamage(damage);
            }



            GameObject instance = Instantiate(particleExplosion, transform.position, Quaternion.identity);
            Destroy(instance, 1f);

            GameObject.Destroy(gameObject);
        }
    }

    public void Explode()
    {
        if (!isExplodingProjectile)
        {
            return;
        }

        List<RaycastHit2D> explosionHits = new List<RaycastHit2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(layerMask);
        Physics2D.CircleCast(transform.position, explosionRadius, Vector2.zero, contactFilter, explosionHits, 10f);

        Debug.Log("explode");
        foreach (RaycastHit2D hit in explosionHits)
        {
            if (hit.collider.CompareTag("Monster"))
            {
                hit.collider.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(damage);
            }
        }


        GameObject instance = Instantiate(particleExplosion, transform.position, Quaternion.identity);
        Destroy(instance, 1f);

        GameObject.Destroy(gameObject);
    }

    public void Start()
    {
        particles.Stop();
        particles.Clear();
    }
    public void Update()
    {
        if (timer >= maxTime)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer >= maxTime)
        {
            particles.Play();
        }
    }

    public static bool IsInLayerMask(int layer, LayerMask _layermask)
    {
        return _layermask == (_layermask | (1 << layer));
    }

}
