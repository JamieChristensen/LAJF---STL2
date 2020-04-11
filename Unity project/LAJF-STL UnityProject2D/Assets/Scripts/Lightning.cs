using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public ParticleSystem trailParticles, collisionParticles;
    private ParticleSystem instanceCollisionParticles;

    private AudioList _audioList;

    private void Start()
    {
        _audioList = FindObjectOfType<AudioList>();
        _audioList.PlayWithVariablePitch(_audioList.lightningStrike);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        instanceCollisionParticles = Instantiate(collisionParticles, transform.position, Quaternion.identity);
        Vector3 trailPosition = new Vector3(trailParticles.transform.position.x, trailParticles.transform.position.y, trailParticles.transform.position.z);
        Vector3 trailSize = new Vector3(trailParticles.transform.localScale.x, trailParticles.transform.localScale.y, trailParticles.transform.localScale.z);
        trailParticles.transform.parent = null;
        trailParticles.transform.position = trailPosition;
        trailParticles.transform.localScale = trailSize;
        instanceCollisionParticles.transform.parent = null;
        //Play Collision sound
        if (collision.gameObject.name == "Player1")
        {
            collision.gameObject.GetComponent<P1Controller>().TakeDamage(10);
            Destroy(gameObject);
            _audioList.PlayWithVariablePitch(_audioList.lightningZap);
        }
        else
        {
            Destroy(gameObject);
            
        }
        Destroy(trailParticles.gameObject, trailParticles.duration);
        Destroy(instanceCollisionParticles.gameObject, instanceCollisionParticles.duration);
    }

}
