using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using STL2.Events;

public class P1Controller : MonoBehaviour
{
    public enum Player1Input
    {
        Horizontal,
        Jump,
        Attack,
        JumpHold,
        DoubleTapLeft,
        DoubleTapRight
    };
    #region INSPECTOR
    public P1Stats runtimePlayerStats;
    public IntEvent playerHPEvent;

    public Rigidbody2D rb;//declares variable to give the player gravity and the ability to interact with physics

    /* controls */
    public KeyCode left, right, jump, attackRanged, attackMelee;

    [SerializeField]
    public Sprite defaultSprite, playerSprite;
    private SpriteRenderer spriteRenderer;

    //White and default materials
    private Material matDefault;
    public Material matWhite;

    public ParticleSystem deathLeadUp, deathExplosion;
    public HealthBar healthBar;
    public Transform particlePoint;


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

    public float projectileSpeed;

    public ChoiceCategory runtimeChoices;

    public List<PlayerItems> playerItems = new List<PlayerItems>();

    private AudioList _audioList;
    public AudioList audioList { get { return _audioList; } }

    [SerializeField]
    private float dropGravityModifier, baseGravity;
    [SerializeField]
    private bool isHoldingJump;

    private bool dashingLeft, dashingRight;
    private float dashTimer;
    [SerializeField]
    private float dashMaxTime = 0.3f;
    [SerializeField]
    private float dashSpeed;

    private bool dashOnCooldown;
    #endregion INSPECTOR

    void Awake()
    {
        _audioList = FindObjectOfType<AudioList>();
        moveDirection = Vector2.right;
        InitializePlayerStats(runtimeChoices.chosenHero); //Use the chosen stats to set baseline of this run.
    }

    private void Start()
    {


        spriteRenderer = GetComponent<SpriteRenderer>();
        matDefault = spriteRenderer.material;
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

        if (dashingLeft || dashingRight)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer > dashMaxTime)
            {
                dashingLeft = false;
                dashingRight = false;
                dashTimer = 0;
            }
        }
        if (dashOnCooldown)
        {
            if (isGrounded)
            {
                dashOnCooldown = false;
            }
        }
        #endregion UpdateCooldowns


        #region MovementModifying
        if (rb != null)
        {
            rb.gravityScale = isHoldingJump ? baseGravity : dropGravityModifier;
        }

        #endregion MovementModifying



        #region UpdateSprites
        spriteRenderer.flipX = moveDirection.x > 0;
        #endregion UpdateSprites
    }


    public void MeleeAttack()
    {

    }

    public void RangedAttack()
    {
        GameObject instance = Instantiate(projectile, transform.position + (((Vector3)moveDirection) * 0.2f), Quaternion.identity);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        //rb.velocity = moveDirection * projectileSpeed;
        rb.AddForce(moveDirection * projectileSpeed, ForceMode2D.Impulse);


        Projectile projInstance = instance.GetComponent<Projectile>();
        projInstance.damage = (int)runtimePlayerStats.baseAttackDamage;

        audioList.PlayWithVariablePitch(audioList.attack1);

        justUsedRangedAttack = true;
    }

    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;
        playerHPEvent.Raise(currentHitPoints);
        if (currentHitPoints > 0) // Show damage effect
        {
            DamageAnimation();
            audioList.PlayWithVariablePitch(audioList.hurt);
        }
        else // Trigger death effect
        {
            DeathAnimation();
            audioList.PlayWithVariablePitch(audioList.deathHero);
        }
    }

    private bool canPlayerAttack()
    {
        //Can have many more conditionals changing this in the future:
        return !justUsedRangedAttack;
    }

    public void ReceiveInput(Player1Input input, float value)
    {
        switch (input)
        {
            case Player1Input.Attack:
                if (!canPlayerAttack())
                {
                    return;
                }

                if (runtimePlayerStats.rangedAttacks)
                {
                    RangedAttack();
                    return;
                }

                if (runtimePlayerStats.meleeAttacks)
                {
                    MeleeAttack();
                    return;
                }

                break;
            case Player1Input.Horizontal:


                rb.velocity = new Vector2(runtimePlayerStats.moveSpeed * value, rb.velocity.y); //if we press the key that corresponds with KeyCode left, then we want the rigidbody to move to the left
                moveDirection = (value > 0.1f) ? Vector2.right :
                    (value < -0.1f) ? Vector2.left : moveDirection;



                if (dashingLeft || dashingRight)
                {
                    float leftD, rightD;
                    leftD = dashingLeft ? 1 : 0;
                    rightD = dashingRight ? 1 : 0;
                    float dashDirection = rightD - leftD;
                    Debug.Log("DASH DIRECTION: " + dashDirection);
                    rb.velocity = new Vector2(dashDirection * dashSpeed, 0);
                }

                if (IsPlayerCloseToObstacle(1.5f))
                {
                    
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }

                break;
            case Player1Input.Jump:

                if (isGrounded)
                {
                    audioList.PlayWithVariablePitch(audioList.jump);
                    rb.AddForce(runtimePlayerStats.jumpForce * Vector2.up, ForceMode2D.Impulse);
                }
                break;
            case Player1Input.JumpHold:
                if (value == 1)
                {
                    isHoldingJump = true;
                }
                else
                {
                    isHoldingJump = false;
                }
                break;

            case Player1Input.DoubleTapLeft:
                if (isGrounded || dashOnCooldown)
                {
                    return;
                }
                dashingLeft = true;
                dashOnCooldown = true;
                //dashTimer = 0;
                break;
            case Player1Input.DoubleTapRight:
                if (isGrounded || dashOnCooldown)
                {
                    return;
                }
                dashingRight = true;
                dashOnCooldown = true;
                //dashTimer = 0;
                break;

        }


    }

    private bool IsPlayerCloseToObstacle(float range)
    {
        //Has to collisioncheck for walls/ground after every round of movement-input.
        //Check if there is a wall on the side the player is moving:
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, range, obstacles);
        // If it hits something...
        if (hit.collider != null && (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Ground") || hit.transform.CompareTag("Cage")))
        {
            return true;
        }

        return false;
    }

    public void UpdatePlayerStats()
    {
        List<PlayerItems> chosenItems = runtimeChoices.playerItems;

        foreach (PlayerItems playerItem in chosenItems)
        {
            //Skip the item if it's already in the players possession:
            if (playerItems.Contains(playerItem))
            {
                continue;
            }
            runtimePlayerStats.maxHitPoints += playerItem.healthModifier;
            currentHitPoints += playerItem.healthModifier;

            runtimePlayerStats.moveSpeed += playerItem.speedModifier;

            runtimePlayerStats.baseAttackDamage += playerItem.damageModifier;
        }

        playerItems.AddRange(chosenItems.Except(playerItems)); //Add new items to playeritems
        //Debug.Log("Updated runtime playerstats with new item!");

        //Strictly speaking only necessary if playerHP actually changed here, but for good measure:
        playerHPEvent.Raise(currentHitPoints);
    }

    public void InitializePlayerStats(P1Stats baselineStats)
    {
        runtimePlayerStats.baseAttackDamage = baselineStats.baseAttackDamage;
        runtimePlayerStats.maxHitPoints = baselineStats.maxHitPoints;
        runtimePlayerStats.moveSpeed = baselineStats.moveSpeed;
        rangedCooldownMaxTime = baselineStats.attackRate;
        runtimePlayerStats.jumpForce = baselineStats.jumpForce;
        runtimePlayerStats.rangedAttacks = baselineStats.rangedAttacks;
        runtimePlayerStats.meleeAttacks = baselineStats.meleeAttacks;
        if (runtimeChoices.chosenHero.characterSprite != null)
        {
            playerSprite = runtimeChoices.chosenHero.characterSprite;
        }
        else
        {
            playerSprite = defaultSprite;
        }
        GetComponent<SpriteRenderer>().sprite = playerSprite;



        currentHitPoints = baselineStats.startingHitPoints;
        playerHPEvent.Raise(currentHitPoints);
        //healthBar.VisualiseHealthChange(baselineStats.startingHitPoints);
    }

    public void DamageAnimation()
    {
        // Add white flash
        spriteRenderer.material = matWhite;
        Invoke("ResetMaterial", 0.1f);
    }

    void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    public void DeathAnimation() // Add Particle Burst
    {
        Destroy(rb);
        ParticleSystem instance = Instantiate(deathLeadUp, particlePoint.position, particlePoint.rotation);
        healthBar.transform.parent = null;
        Destroy(instance.gameObject, instance.duration);
        Invoke("DeathExplode", 1);
    }

    public void DeathExplode() // Add Particle Burst
    {
        audioList.explosion.Play();

        ParticleSystem instance = Instantiate(deathExplosion, particlePoint.position, particlePoint.rotation);
        Destroy(gameObject);
        Destroy(instance.gameObject, instance.duration);
    }


}
