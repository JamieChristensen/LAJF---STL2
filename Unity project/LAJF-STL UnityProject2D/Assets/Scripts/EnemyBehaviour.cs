using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class EnemyBehaviour : MonoBehaviour
{
    private GameManager gameManager;
    public Rigidbody2D rb2;
    public GameObject target;
    public Enemy agent;
    public GameObject bulletObj;
    public HealthBar healthBar;

    private int currentHealth;
    private float cooldownTimer;
    [SerializeField]
    private float projectileSpeed = 1;
    [SerializeField] private int monsterNumber = 1;

    [SerializeField]
    private VoidEvent monsterDied;

    public Transform particlePoint;
    public ParticleSystem deathLeadUp, deathExplosion;
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    //White and default materials
    private Material matDefault;
    public Material matWhite;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        matDefault = spriteRenderer.material;
        gameManager = FindObjectOfType<GameManager>();
        target = GameObject.FindGameObjectWithTag("Player");
        currentHealth = agent.health;
        cooldownTimer = 0;
    }

    void FixedUpdate()
    {
        #region OnDeath
        // Am I alive?
        if (currentHealth <= 0)
        {
            gameObject.layer = 15;
            rb2.AddForce(new Vector2(UnityEngine.Random.Range(-3f, 3f) * 10, UnityEngine.Random.Range(3, 9f) * 10), ForceMode2D.Impulse);
            rb2.AddTorque(50000, ForceMode2D.Impulse);
            rb2.gravityScale = 0f;

           // Invoke("DeathAnimation", 0.2f);
            monsterDied.Raise();



            if (UnityEngine.Random.Range(0, 10f) > 6)
            {
                GameObject.Destroy(gameObject.GetComponent<Collider2D>());
            }
            GameObject.Destroy(this);
            return;
        }
        #endregion OnDeath
        float distance;
        if (target.gameObject != null)
        {
            distance = (target.transform.position - gameObject.transform.position).magnitude;
        }
        else
        { distance = 0; }
        
        // start of behavior tree here

        if (agent.range >= distance)
        {
            if (cooldownTimer <= 0)
            {
                Debug.Log("attacking");
                cooldownTimer = agent.attackSpeed;
                Attack(agent.attackType);
                return;
            }
        }
        if (target.gameObject != null)
        {
            MoveTowards(target.transform);
        }
        
        cooldownTimer -= Time.deltaTime;
    }

    private void Attack(string attackType)
    {
        if (gameManager.canMonsterMove[monsterNumber - 1])
        {
            if (attackType == "melee")
                MeleeAttack();
            if (attackType == "range")
                RangedAttack();
        }
    }

    private void MoveTowards(Transform tf)
    {
        if (gameManager.canMonsterMove[monsterNumber - 1])
        {
            Vector2 direction = (tf.position - transform.position).normalized;
            rb2.velocity = new Vector2(direction.x * agent.speed, rb2.velocity.y);
        }
        
    }

    void MeleeAttack()
    {
        // Vector2 direction = (target.transform.position - transform.position);
        // Collider2D attack = RaycastHit2D();
        throw new NotImplementedException("melee attack");
    }

    void RangedAttack()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletObj, transform.position + (((Vector3)direction) * 0.2f), Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        bullet.GetComponent<Projectile>().damage = agent.damage;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.VisualiseHealthChange(currentHealth);
        DamageAnimation();
    }

    public void OnPlayerDamaged(int PlayerHealth)
    {
        if (PlayerHealth<=0)
        {

        }
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
    /*
    public void DeathAnimation() // Add Particle Burst
    {
        Destroy(rb);
        Instantiate(deathLeadUp, particlePoint.position, particlePoint.rotation);
        healthBar.transform.parent = null;
        Invoke("DeathExplode", 1);
    }

    public void DeathExplode() // Add Particle Burst
    {
        Instantiate(deathExplosion, particlePoint.position, particlePoint.rotation);
        Destroy(gameObject);

    }
    */
}
