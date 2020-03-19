using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public Rigidbody2D rb2;
    public GameObject target;
    public Enemy agent;
    public GameObject bulletObj;

    private float currentHealth;
    private int cooldownTimer;
    private const float PROJECTILE_SPEED = 5;
    


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("P1");
        currentHealth = agent.health;
        cooldownTimer = 0;
    }

    void FixedUpdate()
    {
        // Am I alive?
        if (currentHealth < 0)
        {
            return;
        }
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
        cooldownTimer--;
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
        Vector2 direction = tf.position - transform.position;
        rb2.velocity = ((direction).normalized * agent.speed);
    }

    void MeleeAttack()
    {
        // Vector2 direction = (target.transform.position - transform.position);
        // Collider2D attack = RaycastHit2D();
        throw new NotImplementedException("melee attack");
    }

    void RangedAttack()
    {
       Vector2 direction = (target.transform.position - transform.position);
       GameObject bullet = Instantiate(bulletObj, transform.position + (((Vector3)direction) * 0.2f), Quaternion.identity);
       bullet.GetComponent<Rigidbody2D>().velocity = direction * PROJECTILE_SPEED;
       // bullet.damage = agent.damage; 
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
