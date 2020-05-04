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

    protected int currentHealth;
    public int maxHealth;
    [SerializeField]
    private float projectileSpeed = 1; //Public so it can be accessed by Orb.cs ¯\_(ツ)_/¯
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
    public Rigidbody2D rb;
    //White and default materials
    public Material matDefault;
    public Material matWhite;
    public Enemy agent;
    public List<EnemyModifier> modifiers;
    private AudioList _audioList;
    public AudioList audioList { get { return _audioList; } }

    public bool alreadyDied = false;

    [ColorUsage(true, true)]
    public Color outline;
    [Range(0, 0.1f)]
    public float outlineThiccness;

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

    void Update()
    {
        spriteRenderer.material.SetTexture("_MainTex", agent.sprite.texture);
        spriteRenderer.material.SetFloat("_Thickness", outlineThiccness);
        spriteRenderer.material.SetColor("_Color", outline);
    }


    void FixedUpdate()

    {
        if (isPaused)
            return;

        // checks if it should die
        Die();

        if (!alreadyDied)
        {
            MoveToTarget();
            AttackIfReady();
        }
    }

    protected virtual void MoveToTarget()
    {
        if (target.gameObject != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rb2.velocity = new Vector2(direction.x * agent.speed, rb2.velocity.y);
        }
    }

    protected virtual void MeleeAttack()
    {
        // Vector2 direction = (target.transform.position - transform.position);
        // Collider2D attack = RaycastHit2D();
        throw new NotImplementedException("melee attack");
    }

    protected virtual void RangedAttack()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletObj, transform.position + (((Vector3)direction) * 0.2f), Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        bullet.GetComponent<Projectile>().damage = agent.damage;
    }

    protected virtual void Die()
    {

        if (!CanDie())
        {
            return;
        }

        gameObject.layer = 15;
        rb2.AddForce(new Vector2(UnityEngine.Random.Range(-3f, 3f) * 10, UnityEngine.Random.Range(3, 9f) * 10), ForceMode2D.Impulse);
        rb2.AddTorque(50, ForceMode2D.Impulse);
        rb2.gravityScale = 0f;

        Invoke("DeathAnimation", 0.2f);
        if (audioList == null)
        {
            _audioList = FindObjectOfType<AudioList>();
        }
        if (audioList != null)
        {
            audioList.PlayWithVariablePitch(audioList.deathEnemy);
        }

        StartCoroutine(DelayDeathAnnouncement(1.1f));


        if (UnityEngine.Random.Range(0, 10f) > 6)
        {
            GameObject.Destroy(gameObject.GetComponent<Collider2D>());
        }
        alreadyDied = true;

    }

    private bool CanDie()
    {
        return currentHealth <= 0 && !alreadyDied;
    }

    protected void AttackIfReady()
    {
        string attackType = agent.attackType;
        if (agent.range >= GetTargetDistance())
        {
            if (cooldownTimer <= 0)
            {
                cooldownTimer = agent.attackSpeed;
                if (gameManager == null)
                {
                    gameManager = FindObjectOfType<GameManager>();
                }
                if (gameManager.canMonsterMove[monsterNumber - 1])
                {
                    if (attackType == "melee")
                        MeleeAttack();
                    if (attackType == "range")
                        RangedAttack();
                }
                return;
            }
        }
        cooldownTimer -= Time.deltaTime;
    }

    public float GetTargetDistance()
    {
        float distance;
        if (target.gameObject != null)
        {
            distance = (target.transform.position - gameObject.transform.position).magnitude;
        }
        else
        { distance = 0; }
        return distance;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.GetCurrentHP(currentHealth);
        DamageAnimation();
        if (audioList != null)
        {
            audioList.PlayWithVariablePitch(audioList.hurt);
        }
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

    private void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    public void DeathAnimation() // Add Particle Burst
    {
        rb.velocity = Vector2.zero;
        Destroy(rb);
        ParticleSystem instance = Instantiate(deathLeadUp, particlePoint.position, particlePoint.rotation);
        healthBar.transform.parent = null;
        Invoke("DeathExplode", 1);
        Destroy(instance.gameObject, instance.duration);
    }

    public void DeathExplode() // Add Particle Burst
    {
        if (audioList == null)
        {
            _audioList = FindObjectOfType<AudioList>();
        }
        if (audioList != null)
        {
            audioList.explosion.Play();
        }
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
    public virtual void InitalizeEnemy()
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
        maxHealth = currentHealth;

        spriteRenderer.material.SetTexture("_MainTex", agent.sprite.texture);
        spriteRenderer.material.SetFloat("_Thickness", 0.05f);
    }

    public void InitalizeEnemy(Enemy _agent, EnemyModifier[] _modifiers)
    {
        agent = _agent;
        spriteRenderer.sprite = agent.sprite;
        name = agent.GenerateName(_modifiers.ToList());
        nameUI.SetText(name);

        currentHealth = agent.health;
        maxHealth = currentHealth;
        healthBar.UpdateHPValues(currentHealth, maxHealth);
        foreach (EnemyModifier modifier in _modifiers)
        {
            ApplyModifier(modifier);
        }

        spriteRenderer.material.SetTexture("_MainTex", agent.sprite.texture);
        spriteRenderer.material.SetFloat("_Thickness", 0.05f);

        healthBar.UpdateHPValues(currentHealth, maxHealth);
    }

    IEnumerator DelayDeathAnnouncement(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Length of enemybehaviours: " + FindObjectsOfType<EnemyBehaviour>().Length);

        int amountOfEnemiesAlive = FindObjectsOfType<EnemyBehaviour>().Where(x => x.alreadyDied == false).Count();

        if (amountOfEnemiesAlive <= 0)
        {
            monsterDied.Raise();
        }
    }

    public void SetupHealthBar()
    {
        healthBar.VisualiseHealthChange(agent.health);
    }

    private void ApplyModifier(EnemyModifier _modifier)
    {
        //Do anything else modifiers do:
        //  throw new NotImplementedException();
    }

    private void ShoulderCannonModifier()
    {

    }
    private void BlessedModifier()
    {

    }
    private void AngryModifier()
    {

    }

}
