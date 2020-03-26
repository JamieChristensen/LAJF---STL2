using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class EnemyBehaviour : MonoBehaviour
{

    public Rigidbody2D rb2;
    public GameObject target;
    public Enemy agent;
    public GameObject bulletObj;

    private float currentHealth;
    private float cooldownTimer;
    [SerializeField]
    private float projectileSpeed = 1;

    [SerializeField]
    private VoidEvent monsterDied;


    // Start is called before the first frame update
    void Start()
    {
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

            monsterDied.Raise();

            if (UnityEngine.Random.Range(0, 10f) > 6)
            {
                GameObject.Destroy(gameObject.GetComponent<Collider2D>());
            }
            GameObject.Destroy(this);
            return;
        }
        #endregion OnDeath

        float distance = (target.transform.position - gameObject.transform.position).magnitude;
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
        MoveTowards(target.transform);
        cooldownTimer -= Time.deltaTime;
    }

    private void Attack(string attackType)
    {
        if (attackType == "melee")
            MeleeAttack();
        if (attackType == "range")
            RangedAttack();
    }

    private void MoveTowards(Transform tf)
    {
        Vector2 direction = (tf.position - transform.position).normalized;
        rb2.velocity = new Vector2(direction.x * agent.speed, rb2.velocity.y);
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
    }
}
