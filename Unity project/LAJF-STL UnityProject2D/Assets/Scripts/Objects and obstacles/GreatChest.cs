using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class GreatChest : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public float magnitudeOfScreenshakeOnWallMultiplier;

    [SerializeField]
    private Sprite[] sprites = new Sprite[3];
    private SpriteRenderer spriteRenderer;

    private Vector3 inactivePosition; //Position from which the box drops

    private bool finishedAnimation = false;
    private bool playerOpenedBox = false;
    private bool raisedEvent = false;
    [SerializeField]
    private VoidEvent whenPlayerOpenedBox;
    public VoidEvent narratorHit;

    public ParticleSystem wealthGlow;
    private ParticleSystem instanceWealthGlow;
    private CameraShake cameraShake;
    private bool shakedGround;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraShake = FindObjectOfType<CameraShake>();
        inactivePosition = transform.position;
        GetGlow();
    }

    void Update()
    {
        if (playerOpenedBox)
        {
            spriteRenderer.sprite = sprites[sprites.Length - 1]; //Snap to opened sprite. (no animation yet)
            //animate and whathaveyounot, then raise event.
            finishedAnimation = true; //Obviously needs to animate first.
        }

        if (finishedAnimation && !raisedEvent)
        {
            raisedEvent = true;
            whenPlayerOpenedBox.Raise();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //Prevent constantly re-opening box on new collisions.
        if (playerOpenedBox)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            playerOpenedBox = true;
        }

        if (collision.gameObject.CompareTag("Narrator"))
        {
            narratorHit.Raise();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!shakedGround)
            {
                cameraShake.StartShake(cameraShake.shakePropertyOnChestHit);
                shakedGround = true;
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            float angle = Mathf.Sign(transform.position.x - collision.transform.position.x) == 1 ? 0 : 180;
            float magnitude = Mathf.Clamp(rb.velocity.magnitude, 1, 10);

            //0.3f worked fine as a "strength" value for properties.
            cameraShake.StartShake(new CameraShake.Properties(angle, magnitudeOfScreenshakeOnWallMultiplier*magnitude, 20, 0.3f, 0, 0.6f, 0));

        }
    }

    public void OnPlayerFinishChallenge()
    {
        rb.gravityScale = 1;
    }

    public void ReInitialize()
    {
        //Reset physics, position.
        shakedGround = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        Vector3 deltaVector = new Vector3(Random.Range(0, 40), 0, 0);
        transform.position = inactivePosition - deltaVector;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        //reset bools used to raise event and animate.
        finishedAnimation = false;
        playerOpenedBox = false;
        raisedEvent = false;

        //Reset used sprite
        spriteRenderer.sprite = sprites[0];

        GetGlow();
    }

    public void SetInactivePosition(Vector3 newPosition)
    {
        inactivePosition = newPosition;
    }

    public void RemoveGlow()
    {
        Destroy(instanceWealthGlow.gameObject);
    }

    public void GetGlow()
    {
        Debug.Log("Glow is coming up!");
        instanceWealthGlow = Instantiate(wealthGlow, gameObject.transform);
    }

}
