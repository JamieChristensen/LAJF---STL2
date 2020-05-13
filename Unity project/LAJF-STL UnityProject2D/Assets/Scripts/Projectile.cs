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
                Debug.Log(collided.ToString());
                //Apologies for shit code, I have to check somehow, and this is the best I can think of at 00:40 tonight:
                if (collided.GetComponent<Orb>() != null)
                {
                    collided.GetComponent<Orb>().TakeDamage(damage);
                }
                else if (collided.GetComponent<EnemyBehaviour>() != null)
                {
                    collided.GetComponent<EnemyBehaviour>().TakeDamage(damage);
                }
                else if (collided.GetComponent<AgileEnemy>() != null)
                {
                    collided.GetComponent<AgileEnemy>().TakeDamage(damage);
                }
                else if (collided.GetComponent<OrbEnemy>() != null)
                {
                    collided.GetComponent<OrbEnemy>().TakeDamage(damage);
                }
            }

            if (collided.CompareTag("Player"))
            {
                collided.GetComponent<P1Controller>().TakeDamage(damage);
            }



            GameObject instance = Instantiate(particleExplosion, coll.GetContact(0).point, Quaternion.identity);
            Destroy(instance, 1f);

            GameObject.Destroy(gameObject);
        }
    }

    public void CombatEndedExplode()
    {
        GameObject instance = Instantiate(particleExplosion, transform.position, Quaternion.identity);
        Destroy(instance, 1f);

        GameObject.Destroy(gameObject);
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

        foreach (RaycastHit2D hit in explosionHits)
        {
            if (hit.collider.CompareTag("Monster"))
            {

                //Apologies for shit code, I have to check somehow, and this is the best I can think of at 00:40 tonight:
                if (hit.collider.GetComponent<Orb>() != null)
                {
                    hit.collider.GetComponent<Orb>().TakeDamage(damage);
                }
                else if (hit.collider.GetComponent<EnemyBehaviour>() != null)
                {
                    hit.collider.GetComponent<EnemyBehaviour>().TakeDamage(damage);
                }
                else if (hit.collider.GetComponent<AgileEnemy>() != null)
                {
                    hit.collider.GetComponent<AgileEnemy>().TakeDamage(damage);
                }
                else if (hit.collider.GetComponent<OrbEnemy>() != null)
                {
                    hit.collider.GetComponent<OrbEnemy>().TakeDamage(damage);
                }
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
