using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Controller : MonoBehaviour
{

    #region INSPECTOR
    public GameManager findGameManager;


    public Rigidbody2D rb;//declares variable to give the player gravity and the ability to interact with physics

    /* controls */
    public KeyCode left, right, jump, attackRanged, attackMelee;

    /* Player Stats */
    public P1Stats p1Stats;

    [SerializeField] private string _name; // name of the character
    [SerializeField] private string _description; // description of character
    [SerializeField] private string _trait; // the traits of choosing this character

    [SerializeField] private float _moveSpeed; //the speed the player is able to move with
    [SerializeField] private float _jumpForce; //how high the player jump

    [SerializeField] private float _damageReduction; // the damage reduction of the character
    [SerializeField] private int _maxHitPoints; // maximum amount of HP

    [SerializeField] private float _attackDamage; // the damage of the character
    [SerializeField] private float _attackRate; // minimum time between attacks
    [SerializeField] private bool _rangedAttacks; // can perform ranged attacks
    [SerializeField] private bool _meleeAttacks; // can perform melee attacks


    /* Player Status */
    public int currentHitPoints; // current amount of HP
    public bool attackIsOnCooldown; // is the attack on cooldown
    public bool isGrounded; //it's either true or false if the player is on the ground

    #endregion

    void Awake()
    {
        /* Get the player specific stats from the ScriptableObject  */
        _name = p1Stats.name; // get name
        _description = p1Stats.description; // get description
        _trait = p1Stats.trait; // get trait
        _moveSpeed = p1Stats.moveSpeed; // get movement speed
        _jumpForce = p1Stats.jumpForce; // get jump force
        _damageReduction = p1Stats.baseDamageReduction; // get damage reduction
        _maxHitPoints = p1Stats.maxHitPoints; // get max HP
        currentHitPoints = p1Stats.startingHitPoints; // get starting health
        _attackDamage = p1Stats.baseAttackDamage; // get attack damage
        _attackRate = p1Stats.attackRate; // get attack rate
        _rangedAttacks = p1Stats.rangedAttacks; // can the character use range attacks?
        _meleeAttacks = p1Stats.meleeAttacks; // can the character use melee attacks?

    }


    private void Update()
    {
        if (Input.GetKey(left))
        {
            rb.velocity = new Vector2(-_moveSpeed, rb.velocity.y); //if we press the key that corresponds with KeyCode left, then we want the rigidbody to move to the left
        }
        else if (Input.GetKey(right))
        {
            rb.velocity = new Vector2(_moveSpeed, rb.velocity.y); //if we press the key that corresponds with KeyCode right, then we want the rigidbody to move to the right
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKeyDown(jump) && isGrounded) //if the button is just pressed (and not held down), then force will be added, so the player jumps into the air if the player is on the ground when pressed
        {
            rb.velocity = new Vector2(rb.velocity.x, _jumpForce);
            
        }
        if (Input.GetKeyDown(attackMelee) && _meleeAttacks == true) // if pressed the keybinding for melee attack & the character can perform melee attacks
        {
            MeleeAttack();
        }


        if (Input.GetKeyDown(attackRanged) && _rangedAttacks == true) // if pressed the keybinding for ranged attack & the character can perform ranged attacks
        {
            RangedAttack();
        }
    }



    public void MeleeAttack()
    {

    }

    public void RangedAttack()
    {

    }

  


    void FixedUpdate()
    {
        
    }

}
