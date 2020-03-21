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
    public void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject collided = coll.gameObject;
        if (IsInLayerMask(collided.layer, layerMask))
        {
            if (collided.CompareTag("Monster")) {
                //My best guess of what Luca would implement:
                collided.GetComponent<EnemyBehaviour>().TakeDamage(damage);
            }

            if (collided.CompareTag("Player")){
                collided.GetComponent<P1Controller>().TakeDamage(damage);
            }

            GameObject.Destroy(gameObject);
        }
    }
    public void Start(){
        particles.Stop();
        particles.Clear();
    }
    public void Update()
    {
        if (timer >= maxTime){
            return;
        }
        timer += Time.deltaTime;
        if(timer >= maxTime){
            particles.Play();
        }
    }

    public static bool IsInLayerMask(int layer, LayerMask _layermask)
    {
        return _layermask == (_layermask | (1 << layer));
    }

}
