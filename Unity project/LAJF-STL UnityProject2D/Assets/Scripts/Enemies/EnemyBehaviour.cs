using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using STL2.Events;
using TMPro;

public class EnemyBehaviour : MonoBehaviour, IPausable
{
    private GameManager gameManager;
    public Rigidbody2D rb2;
    public GameObject target;
    public GameObject bulletObj;
    public HealthBar healthBar;
    public new string name;
    public TextMeshProUGUI nameUI;

    private int currentHealth;
    [SerializeField]
    private float projectileSpeed = 1;
    private float cooldownTimer;
    [SerializeField]
    private int monsterNumber = 1;

    [SerializeField]
    private VoidEvent monsterDied;

    public bool isPaused;
    public Transform particlePoint;
    public ParticleSystem deathLeadUp, deathExplosion;
    public SpriteRenderer spriteRenderer;
    public ChoiceCategory runtimeChoices;
    private Rigidbody2D rb;
    //White and default materials
    private Material matDefault;
    public Material matWhite;
    public Enemy agent;
    public List<EnemyModifier> modifiers;
    private AudioList _audioList;
    public AudioList audioList { get { return _audioList; } }

    bool alreadyDied = false;

    // Start is called before the first frame update
    void Start()
    {
        // InitalizeEnemy();
        _audioList = FindObjectOfType<AudioList>();
        rb = GetComponent<Rigidbody2D>();
        matDefault = spriteRenderer.material;
        gameManager = FindObjectOfType<GameManager>();
        target = GameObject.FindGameObjectWithTag("Player");
        cooldownTimer = 0;
    }

    void FixedUpdate()
    {

        if (isPaused)
            return;
        #region OnDeath
        // Am I alive?
        if (currentHealth <= 0)
        {
            if (!alreadyDied)
            {
                gameObject.layer = 15;
                rb2.AddForce(new Vector2(UnityEngine.Random.Range(-3f, 3f) * 10, UnityEngine.Random.Range(3, 9f) * 10), ForceMode2D.Impulse);
                rb2.AddTorque(50, ForceMode2D.Impulse);
                rb2.gravityScale = 0f;

                Invoke("DeathAnimation", 0.2f);
                audioList.PlayWithVariablePitch(audioList.deathEnemy);
                StartCoroutine(DelayDeathAnnouncement(1.1f));

                if (UnityEngine.Random.Range(0, 10f) > 6)
                {
                    GameObject.Destroy(gameObject.GetComponent<Collider2D>());
                }
                // GameObject.Destroy(this);
                alreadyDied = true;
            }

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
        healthBar.GetCurrentHP(currentHealth);
        DamageAnimation();
        audioList.PlayWithVariablePitch(audioList.hurt);
    }

    public void OnPlayerDamaged(int PlayerHealth)
    {
        if (PlayerHealth <= 0)
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

    public void DeathAnimation() // Add Particle Burst
    {
        Destroy(rb);
        ParticleSystem instance = Instantiate(deathLeadUp, particlePoint.position, particlePoint.rotation);
        healthBar.transform.parent = null;
        Invoke("DeathExplode", 1);
        Destroy(instance.gameObject, instance.duration);
    }

    public void DeathExplode() // Add Particle Burst
    {
        audioList.explosion.Play();
        ParticleSystem instance = Instantiate(deathExplosion, particlePoint.position, particlePoint.rotation);
        Destroy(instance.gameObject, instance.duration);
        Destroy(gameObject);

    }

    public bool IsPaused()
    {
        return isPaused;
    }
    public void Pause()
    {
        isPaused = true;
    }

    public void UnPause()
    {
        isPaused = false;
    }

    // previous params for this function (Enemy enemy, List<EnemyModifier> enemyModifiers)
    public void InitalizeEnemy()
    {
        List<EnemyModifier> enemyModifiers = new List<EnemyModifier>();
        agent = runtimeChoices.enemies[runtimeChoices.enemies.Count - 1];
        modifiers.Add(runtimeChoices.enemyModifiers[runtimeChoices.enemyModifiers.Count - 1]);
        // get info from runtime stats from somewhere 
        // enemy = runtimeStats.whatever.enemy
        // EnemyModifiers = runtimeStats.whatever.modifer
        // NB right now everything is assigned by hardcode through this function
        spriteRenderer.sprite = agent.sprite;
        name = agent.GenerateName(modifiers);
        nameUI.SetText(name);
        currentHealth = agent.health;

    }
    public void InitalizeEnemy(Enemy _agent, EnemyModifier[] _modifiers)
    {
        agent = _agent;
        spriteRenderer.sprite = agent.sprite;
        name = agent.GenerateName(_modifiers.ToList());
        nameUI.SetText(name);

        currentHealth = agent.health;
        healthBar.UpdateHPValues(currentHealth, agent.health);
        foreach (EnemyModifier modifier in _modifiers)
        {
            ApplyModifier(modifier);
        }

        healthBar.UpdateHPValues(currentHealth, agent.health);
    }

    private void ApplyModifier(EnemyModifier _modifier)
    {
        //Do anything else modifiers do:
      //  throw new NotImplementedException();
    }

    IEnumerator DelayDeathAnnouncement(float delay)
    {
        yield return new WaitForSeconds(delay);
        monsterDied.Raise();
    }


    public void SetupHealthBar()
    {
        healthBar.VisualiseHealthChange(agent.health);
    }

}
