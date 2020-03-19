using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class P1Controller : MonoBehaviour
{

    #region INSPECTOR
    public IntEvent playerHPEvent;

    public Rigidbody2D rb;//declares variable to give the player gravity and the ability to interact with physics

    /* controls */
    public KeyCode left, right, jump, attackRanged, attackMelee;

    /* Player Stats */
    public P1Stats playerStats;


    /* Player Status */
    public int currentHitPoints; // current amount of HP
    public bool attackIsOnCooldown; // is the attack on cooldown
    public bool isGrounded; //it's either true or false if the player is on the ground

    private Vector2 moveDirection;
    
    public GameObject projectile;

    #endregion

    void Awake()
    {
        moveDirection  = Vector2.right;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-playerStats.moveSpeed, rb.velocity.y); //if we press the key that corresponds with KeyCode left, then we want the rigidbody to move to the left
            moveDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(playerStats.moveSpeed, rb.velocity.y); //if we press the key that corresponds with KeyCode right, then we want the rigidbody to move to the right
            moveDirection = Vector2.right;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKeyDown(jump) && isGrounded) //if the button is just pressed (and not held down), then force will be added, so the player jumps into the air if the player is on the ground when pressed
        {
            rb.velocity = new Vector2(rb.velocity.x, playerStats.jumpForce);
            
        }
        if (Input.GetKeyDown(attackMelee) && playerStats.meleeAttacks == true) // if pressed the keybinding for melee attack & the character can perform melee attacks
        {
            MeleeAttack();
        }


        if (Input.GetKeyDown(attackRanged) && playerStats.rangedAttacks == true) // if pressed the keybinding for ranged attack & the character can perform ranged attacks
        {
            RangedAttack();
        }
    }



    public void MeleeAttack()
    {

    }

    public void RangedAttack()
    {
        GameObject instance = Instantiate(projectile, transform.position, Quaternion.identity);
        Projectile projInstance = instance.GetComponent<Projectile>();
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        //Projectile speed:
        rb.velocity = moveDirection*100;

        projInstance.damage = 10;

        //Add velocity in movedirection and more.
    }

    public void WhenPlayerHPChanges(){
        //Probably do more than just this.
        playerHPEvent.Raise(currentHitPoints);
    }



}
