using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class P1Controller : MonoBehaviour
{

    #region INSPECTOR
    public P1Stats playerStats;
    public IntEvent playerHPEvent;

    public Rigidbody2D rb;//declares variable to give the player gravity and the ability to interact with physics

    /* controls */
    public KeyCode left, right, jump, attackRanged, attackMelee;

    [SerializeField]
    private SpriteRenderer spriteRenderer;


    /* Player Status */
    public int currentHitPoints; // current amount of HP
    public bool attackIsOnCooldown; // is the attack on cooldown
    public bool isGrounded; //it's either true or false if the player is on the ground

    private Vector2 moveDirection;

    public GameObject projectile;

    public float rangedAttackCooldownTimer = 0;
    public float rangedCooldownMaxTime = 0.2f;
    private bool justUsedRangedAttack = false;

    public LayerMask obstacles;
    #endregion

    void Awake()
    {
        moveDirection = Vector2.right;
    }


    private void Update()
    {
        #region UpdateCooldowns
        if (justUsedRangedAttack)
        {
            rangedAttackCooldownTimer += Time.deltaTime;
            if (rangedAttackCooldownTimer >= rangedCooldownMaxTime)
            {
                justUsedRangedAttack = false;
                rangedAttackCooldownTimer = 0;
            }
        }
        #endregion UpdateCooldowns



        //Move input-handling to input-manager later:
        #region InputsAndMovement
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

        //Check if there is a wall on the side the player is moving:
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 3f, obstacles);
        // If it hits something...
        if (hit.collider != null){
            Debug.Log(hit.transform.gameObject.name);
        }
        if (hit.collider != null && hit.transform.CompareTag("Wall"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }


        if (Input.GetKeyDown(jump) && isGrounded) //if the button is just pressed (and not held down), then force will be added, so the player jumps into the air if the player is on the ground when pressed
        {
            rb.AddForce(playerStats.jumpForce * Vector2.up, ForceMode2D.Impulse);

        }
        if (Input.GetKeyDown(attackMelee) && playerStats.meleeAttacks == true) // if pressed the keybinding for melee attack & the character can perform melee attacks
        {
            MeleeAttack();
        }


        if (Input.GetKeyDown(attackRanged) && playerStats.rangedAttacks == true) // if pressed the keybinding for ranged attack & the character can perform ranged attacks
        {
            if (!justUsedRangedAttack)
            {
                RangedAttack();
            }
        }
        #endregion InputsAndMovement

        #region UpdateSprites
        spriteRenderer.flipX = moveDirection.x > 0;
        #endregion UpdateSprites
    }




    public void MeleeAttack()
    {

    }

    public void RangedAttack()
    {
        GameObject instance = Instantiate(projectile, transform.position + (((Vector3)moveDirection)*0.2f), Quaternion.identity);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        rb.velocity = moveDirection * 100;

        
        Projectile projInstance = instance.GetComponent<Projectile>();
        projInstance.damage = 10;

        //Add velocity in movedirection and more.

        justUsedRangedAttack = true;
    }

    public void TakeDamage(int damage){
        currentHitPoints -= damage;
    }

    public void WhenPlayerHPChanges()
    {
        //Probably do more than just this.
        playerHPEvent.Raise(currentHitPoints);
    }



}
