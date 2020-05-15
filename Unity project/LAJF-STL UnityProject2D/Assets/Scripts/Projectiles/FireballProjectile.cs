using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{

    public LayerMask layer;
    public int damage;
    public Rigidbody2D rb2;
    public GameObject fireballParent;

    private float initalBounce = 10f;
    private float minBounce = 10f;
    private int bounceCount;
    public int bounceLimit = 3;
    public Vector3 rotate;

    private CameraShake cameraShake;

    private float magnitudeOfScreenshakeOnWallMultiplier = 0.07f;

    // Start is called before the first frame update
    void Start()
    {
        cameraShake = FindObjectOfType<CameraShake>();
        rotate = new Vector3(0, 0, Random.Range(1, 3));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotate);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collided = collision.gameObject;
        bounceCount++;

        if (bounceCount > bounceLimit)
            Destroy(fireballParent);

        Vector2 direction = rb2.velocity.normalized;
        // this adds a random direction to the bounce so that it doesnt bounces straight up! 
        Vector2 random = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        float bounceEffect = ((initalBounce / bounceCount) + minBounce);
        rb2.AddForce((random + Vector2.up) * bounceEffect, ForceMode2D.Impulse);


        //if (Projectile.IsInLayerMask(collision.gameObject.layer, layer))
        //    Debug.Log("Fireball hit!");
        if (collided.CompareTag("Player"))
        {
            collided.GetComponent<P1Controller>().TakeDamage(damage);
        }
        if (collided.CompareTag("Ground") || collided.CompareTag("Wall"))
        {
            float angle = Mathf.Atan2(rb2.velocity.y, rb2.velocity.x);
            float magnitude = Mathf.Clamp(rb2.velocity.magnitude, 1, 10);

            //0.3f worked fine as a "strength" value for properties.
            cameraShake.StartShake(new CameraShake.Properties(angle, magnitudeOfScreenshakeOnWallMultiplier*magnitude, 20, 0.3f, 0, 0.6f, 0));
        }

        // needs to be moved
    }
}
