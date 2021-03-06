﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileEnemy : EnemyBehaviour
{
    // movement
    [Range(0, 1)]
    public float jumpFrequency = 0.002f;
    public float jumpHeight = 20;
    private bool grounded = false;
    public float moveCap = 20f;


    // dodge balancing 
    public int dodgeCooldown = 100;
    public int dodgeTimer = 0;

    // dodge design
    public int dodgeForce = 50;
    public float dodgeDuration = 0.13f;
    public float dodgeRemaining = 0;

    public LayerMask layerMask;

    public bool jumping;
    public float jumpTimer;
    public float upwardsJumpTime;

    protected override void MoveToTarget()
    {

        if (dodgeRemaining > 0)
        {
            dodgeRemaining -= Time.fixedDeltaTime;
            if (dodgeRemaining <= 0)
                rb2.velocity = Vector2.zero;
        }
        // go towards player
        base.MoveToTarget();

        dodgeTimer = UpdateCooldown(dodgeTimer);


        // jump
        if (Random.value < jumpFrequency && grounded)
        {
            if (!jumping)
            {
                jumping = true;
                jumpTimer = 0;
            }


            grounded = false;
        }

        if (jumping)
        {

            jumpTimer += Time.deltaTime;

            if (jumpTimer > upwardsJumpTime)
            {
                rb2.velocity = new Vector2(rb.velocity.x, rb2.velocity.y); //Allow gravity to have effect.
            }
            else
            {
                float jumpForce = Mathf.InverseLerp(upwardsJumpTime, 0, jumpTimer) * Time.deltaTime;
                rb2.velocity = new Vector2(rb.velocity.x, jumpForce*jumpHeight);
                Debug.Log("Effective Y-velocity: " +  jumpForce*jumpHeight);
            }
        }
        if (grounded)
        {
            jumping = false;
            jumpTimer = 0;
            
        }
    }


    protected override void RangedAttack()
    {
        base.RangedAttack();
        Debug.Log("Im a child of enemy behavior");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Dodge(collision);
    }

    private bool CanUseAbility(int cooldown)
    {
        return cooldown == 0 && grounded;
    }

    private int UpdateCooldown(int cooldown)
    {
        return Mathf.Max(--cooldown, 0);
    }

    private void Dodge(Collider2D collider)
    {
        // does the trigger have correct layer, is dodge available and is the object grounded? 
        bool canDodge = Projectile.IsInLayerMask(collider.gameObject.layer, layerMask) && CanUseAbility(dodgeTimer);
        if (!canDodge)
            return;
        // placeholder dodge function
        Vector2 origin = collider.transform.position;
        Vector2 direction = collider.GetComponent<Rigidbody2D>().velocity;
        bool hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);
        if (hit)
        {
            rb2.AddForce((Vector2.up - direction.normalized) * dodgeForce, ForceMode2D.Impulse);
            dodgeRemaining = dodgeDuration;
            dodgeTimer = dodgeCooldown;
            grounded = false;
            RangedAttack();
        }


    }
}
