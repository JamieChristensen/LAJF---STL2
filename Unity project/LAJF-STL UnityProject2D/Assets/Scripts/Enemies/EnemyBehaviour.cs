using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using STL2.Events;
using TMPro;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour, IPausable
{
    // Floating Text
    public GameObject floatingTextPrefab; //, floatingCanvasPrefab;
    public GameObject floatingCanvasParent;
    public GameObject floatingTextInstance;

    private CameraShake cameraShaker;
    private GameManager gameManager;
    public Rigidbody2D rb2;
    public GameObject target;
    public GameObject bulletObj;
    public HealthBar healthBar;
    public new string name;
    public TextMeshProUGUI nameUI;

    public int currentHealth;
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

    public SpriteRenderer angrySpriteRenderer;
    public SpriteRenderer shoulderCannonRenderer;

    public bool hasCannon, isBlessed, isAngry;

    public GameObject tombstone;

    [NonSerialized]
    public bool flipped = true;

    public float desiredDistance = 30f;

    [NonSerialized]
    public bool isSplitted = false;


    // Start is called before the first frame update
    void Start()
    {
        // InitalizeEnemy();
        _audioList = FindObjectOfType<AudioList>();
        cameraShaker = FindObjectOfType<CameraShake>();
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

            float targetSign = Mathf.Sign(target.transform.position.x - transform.position.x);
            float velocityX = targetSign * agent.speed;
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < desiredDistance)
            {
                velocityX *= -1;
            }
            if (distance < desiredDistance + 1f && distance > desiredDistance - 1f)
            {
                velocityX *= 0;
            }


            rb2.velocity = new Vector2(velocityX, rb2.velocity.y);

            #region Rotate
            if (Mathf.Sign(target.transform.position.x - transform.position.x) == -1)
            {
                if (!flipped)
                {
                    spriteRenderer.flipX = false;
                    angrySpriteRenderer.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    Vector3 transAngry = angrySpriteRenderer.transform.localPosition;
                    angrySpriteRenderer.transform.localPosition = new Vector3(transAngry.x * -1, transAngry.y, transAngry.z);

                    shoulderCannonRenderer.transform.localRotation = Quaternion.Euler(0, 180, 45);
                    Vector3 shoulderCannon = shoulderCannonRenderer.transform.localPosition;
                    shoulderCannonRenderer.transform.localPosition = new Vector3(shoulderCannon.x * -1, shoulderCannon.y, shoulderCannon.z);

                    flipped = true;
                }
            }
            else
            {
                if (flipped)
                {
                    spriteRenderer.flipX = true;
                    angrySpriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    Vector3 transAngry = angrySpriteRenderer.transform.localPosition;
                    angrySpriteRenderer.transform.localPosition = new Vector3(transAngry.x * -1, transAngry.y, transAngry.z);

                    shoulderCannonRenderer.transform.localRotation = Quaternion.Euler(0, 0, 45);
                    Vector3 shoulderCannon = shoulderCannonRenderer.transform.localPosition;
                    shoulderCannonRenderer.transform.localPosition = new Vector3(shoulderCannon.x * -1, shoulderCannon.y, shoulderCannon.z);
                    flipped = false;
                }
            }

            #endregion Rotate
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
        if (isSplitted)
        {
            bullet.GetComponent<Projectile>().damage = agent.damage / 2;
        }

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
            #region SetSpritesBasedOnAttackPattern
            //Order of following if-statements matter:
            if (cooldownTimer <= agent.attackSpeed * 0.9f)
            {
                spriteRenderer.sprite = agent.sprite;
                spriteRenderer.material.SetTexture("_MainTex", spriteRenderer.sprite.texture); //Use the newly assigned texture.
            }
            if (cooldownTimer <= 0.7f)
            {
                spriteRenderer.sprite = agent.rampUpForAttackSprite;
                spriteRenderer.material.SetTexture("_MainTex", spriteRenderer.sprite.texture); //Use the newly assigned texture.
            }
            if (cooldownTimer <= agent.attackSpeed * 0.1f)
            {
                spriteRenderer.sprite = agent.attackingSprite;
                spriteRenderer.material.SetTexture("_MainTex", spriteRenderer.sprite.texture); //Use the newly assigned texture.
            }
            #endregion SetSpritesBasedOnAttackPattern

            if (cooldownTimer <= 0)
            {
                cooldownTimer = agent.attackSpeed;

                foreach (EnemyModifier mod in modifiers)
                {
                    if (mod.modifierType == EnemyModifier.ModifierType.Angry)
                    {
                        //Increase attackspeed by up to twice as much, depending on enemyHP (half the time between attacks as normally, when enemy has 0 HP)
                        cooldownTimer = map(currentHealth, 0, maxHealth, agent.attackSpeed / 2, agent.attackSpeed);
                    }
                }



                if (gameManager == null)
                {
                    gameManager = FindObjectOfType<GameManager>();
                }
                if (gameManager.canMonsterMove[monsterNumber - 1])
                {
                    if (hasCannon)
                    {
                        foreach (EnemyModifier modifier in modifiers)
                        {
                            Vector2 velocity = new Vector2(12 * Mathf.Sign(target.transform.position.x - transform.position.x), 14);
                            if (modifier.modifierType == EnemyModifier.ModifierType.ShoulderCannon)
                            {
                                modifiers[0].FireShoulderCannon(velocity, transform.position, 10);
                                velocity += new Vector2(-1, 1) * 2;
                            }
                        }
                    }
                    if (attackType == "melee")
                        MeleeAttack();
                    if (attackType == "range")
                        RangedAttack();
                }
                return;
            }
        }
        else //If outside of range.
        {
            cooldownTimer = agent.attackSpeed;
            spriteRenderer.sprite = agent.sprite;
            spriteRenderer.material.SetTexture("_MainTex", spriteRenderer.sprite.texture); //Use the newly assigned texture.
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
            audioList.PlayWithVariablePitch(audioList.hurtEnemy);
        }

        if (floatingTextPrefab != null)
        {
            ShowFloatingText(damage); // Trigger floating text
            //Debug.Log("Should trigger text");
        }
    }
    #region FloatingText

    public void ShowFloatingText(int damage)
    {
        
        if (floatingCanvasParent == null) // if the canvas is not yet instantiated
        {
            floatingCanvasParent = GameObject.Find("FloatingCanvas"); // find the canvas (parent) for text
        }
        
        float randomX = UnityEngine.Random.Range(-4.5f, 4.5f); // Random position.x
        float randomY = UnityEngine.Random.Range(-1.5f, 4.5f); // Random position.y
        Vector3 randomVector = new Vector3(randomX, randomY, 0); // Random combined position
        floatingTextInstance = Instantiate(floatingTextPrefab, transform.position + randomVector, Quaternion.identity, floatingCanvasParent.transform); // instantiate text object

        floatingTextInstance.GetComponent<FloatingTextEffects>().setText(damage.ToString()); //Sets the text of the text object - the rest will happen in the instance (FloatingTextEffects.cs)
    }

   
    #endregion FloatingText

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
        Destroy(instance.gameObject, instance.main.duration);
    }

    public void DeathExplode() // Add Particle Burst
    {
        #region SpawnGravestone
        GameObject go = Instantiate(tombstone, transform.position, Quaternion.identity);
        go.GetComponent<SpriteRenderer>().material.SetTexture("_Texture2D", agent.texture);
        go.transform.localScale = transform.localScale;
        #endregion SpawnGravestone


        if (audioList == null)
        {
            _audioList = FindObjectOfType<AudioList>();
        }
        if (audioList != null)
        {
            audioList.explosion.Play();
        }
        if (cameraShaker != null)
        {
            cameraShaker.StartShake(cameraShaker.shakePropertyOnMinionDie);
        }
        ParticleSystem instance = Instantiate(deathExplosion, particlePoint.position, particlePoint.rotation);
        if (isBlessed)
        {
            foreach (EnemyModifier modifier in modifiers)
            {
                if (modifier.modifierType == EnemyModifier.ModifierType.Blessed)
                {
                    modifier.Revive(this);
                }
            }
        }
        Destroy(instance.gameObject, instance.main.duration);
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
        //modifiers.Add(runtimeChoices.enemyModifiers[runtimeChoices.enemyModifiers.Count - 1]);
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
        spriteRenderer.material.SetFloat("_Thickness", 0.02f);

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
            modifiers.Add(modifier);
        }

        spriteRenderer.material.SetTexture("_MainTex", agent.sprite.texture);
        spriteRenderer.material.SetFloat("_Thickness", 0.02f);

        healthBar.UpdateHPValues(currentHealth, maxHealth);

        StartCoroutine(UINameFadeOut(4));

    }

    public IEnumerator UINameFadeOut(float delay)
    {
        Vector3 currentSize = Vector3.zero; // start the text size at 0
        Vector3 targetSize = new Vector3(1, 1, 1); // set the target size to 1
        RectTransform textRectTransform = null;

        nameUI = transform.Find("Canvas").transform.Find("EnemyName").GetComponent<TextMeshProUGUI>();
        try
        {
            textRectTransform = nameUI.GetComponent<RectTransform>();
        }
        catch
        {
            Debug.Log("textRectTransform could not be set");
        }

        float timer = 0;
        float timeToLerp = 1f;
        //scaling up
        while (timer < timeToLerp)
        {
            timer += Time.deltaTime;
            currentSize = Vector3.Lerp(currentSize, targetSize, 2 * Time.deltaTime / timeToLerp);
            if (textRectTransform != null)
            {
                textRectTransform.localScale = currentSize;
            }
            
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        currentSize = targetSize;
        targetSize = Vector3.zero; // set the target size to Vector3.zero
        timer = 0;
        while (timer < timeToLerp)
        {
            timer += Time.deltaTime;
            currentSize = Vector3.Lerp(currentSize, targetSize, 2 * Time.deltaTime / timeToLerp);
            if (textRectTransform != null)
            {
                textRectTransform.localScale = currentSize;
            }
            yield return null;
        }
        currentSize = targetSize;
        textRectTransform.localScale = currentSize;
    }

    IEnumerator DelayDeathAnnouncement(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Length of enemybehaviours: " + FindObjectsOfType<EnemyBehaviour>().Length);

        int amountOfEnemiesAlive = FindObjectsOfType<EnemyBehaviour>().Where(x => x.alreadyDied == false).Count();

        if (amountOfEnemiesAlive <= 0 && !isBlessed)
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
        switch (_modifier.modifierType)
        {
            case EnemyModifier.ModifierType.ShoulderCannon:
                _modifier.ApplyShoulderCannonVisuals(this);
                hasCannon = true;
                break;
            case EnemyModifier.ModifierType.Blessed:
                _modifier.ApplyHolyVisuals(this);
                isBlessed = true;
                break;
            case EnemyModifier.ModifierType.Angry:
                _modifier.ApplyAngrySprite(this);
                isAngry = true;
                break;
        }
    }


    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
