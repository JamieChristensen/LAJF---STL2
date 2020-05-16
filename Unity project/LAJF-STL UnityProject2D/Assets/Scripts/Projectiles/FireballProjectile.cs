using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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

    [SerializeField]
    private float magnitudeOfScreenshakeOnWallMultiplier = 0.07f;

    public GameObject fireImpactParticles;
    public float lightDuration;

    public Coroutine reduceIntensityCO;

    private GameObject spawnedObj;
    private Light2D lightOfSpawn;



    // Start is called before the first frame update
    void Start()
    {
        cameraShake = FindObjectOfType<CameraShake>();
        rotate = new Vector3(0, 0, Random.Range(1, 3));
        reduceIntensityCO = null;
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




        //if (Projectile.IsInLayerMask(collision.gameObject.layer, layer))
        //    Debug.Log("Fireball hit!");
        if (collided.CompareTag("Player"))
        {
            collided.GetComponent<P1Controller>().TakeDamage(damage);
        }

        if (collided.CompareTag("Ground") || collided.CompareTag("Wall"))
        {
            Vector3 eulerVecOfImpact;
            if (collided.CompareTag("Ground"))
            {
                eulerVecOfImpact = new Vector3(0, 0, 0);
            }
            else //Collision with wall then:
            {
                eulerVecOfImpact = collided.transform.position.x > transform.position.x ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
            }

            Vector3 point = collision.GetContact(0).point;
            spawnedObj = Instantiate(fireImpactParticles, point, Quaternion.Euler(eulerVecOfImpact));
            lightOfSpawn = spawnedObj.GetComponentInChildren<Light2D>();
            spawnedObj.GetComponent<DestroySelf>().StartReduceIntensity(lightDuration);

            float magnitude = Mathf.Clamp(rb2.velocity.magnitude, 10, 15);

            float angle = Mathf.Sign(transform.position.y - collided.transform.position.y) == 1 ? 90 : 270;

            if (collided.CompareTag("Wall"))
            {
                angle = Mathf.Sign(transform.position.x - collided.transform.position.x) == 1 ? 180 : 0;
            }

            //0.3f worked fine as a "strength" value for properties.
            cameraShake.StartShake(new CameraShake.Properties(angle, magnitudeOfScreenshakeOnWallMultiplier * magnitude, 20, 0.3f, 0, 0.6f, 0));
        }

        if (collided.CompareTag("Player"))
        {
            Vector3 point = collision.GetContact(0).point;
            spawnedObj = Instantiate(fireImpactParticles, point, Quaternion.Euler(transform.rotation.eulerAngles));
            lightOfSpawn = spawnedObj.GetComponentInChildren<Light2D>();
            spawnedObj.GetComponent<DestroySelf>().StartReduceIntensity(lightDuration);
            cameraShake.StartShake(cameraShake.shakePropertyOnMinionEnter);
        }

        Vector2 direction = rb2.velocity.normalized;
        // this adds a random direction to the bounce so that it doesnt bounces straight up! 
        Vector2 random = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        float bounceEffect = ((initalBounce / bounceCount) + minBounce);
        rb2.AddForce((random + Vector2.up) * bounceEffect * rb2.mass, ForceMode2D.Impulse);
        // needs to be moved
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}