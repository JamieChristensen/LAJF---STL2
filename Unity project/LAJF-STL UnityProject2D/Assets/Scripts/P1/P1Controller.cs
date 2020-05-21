using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using STL2.Events;
using UnityEngine.UI;

public class P1Controller : MonoBehaviour
{
    

    public enum Player1Input
    {
        Horizontal,
        Jump,
        Attack,
        UseForceField,
        StopForceField,
        JumpHold,
        DoubleTapLeft,
        DoubleTapRight,
        Explosion
    };

    #region INSPECTOR

    // Floating Text
    public GameObject floatingTextPrefab; //, floatingCanvasPrefab;
    public GameObject floatingCanvasParent;
    public GameObject floatingTextInstance;

    public P1Stats runtimePlayerStats;
    public IntEvent playerHPEvent;

    public Rigidbody2D rb;//declares variable to give the player gravity and the ability to interact with physics

    [Header("Controls")]
    /* controls */
    //public KeyCode left, right, jump, attackRanged, attackMelee, forceField;


    [Header("Visuals and misc.:")]
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
    public GameObject muzzlePrefab;
    public Transform firePoint;
    private float firePointoffset; //used to offset 

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
    bool isMovingRight;

    private bool dashingLeft, dashingRight;
    private float dashTimer;
    [SerializeField]
    private float dashMaxTime = 0.3f;
    [SerializeField]
    private float dashSpeed;
    private bool dashOnCooldown;
    private bool hasShotgun = false;
    public bool hasGatlingGun = false;

    [SerializeField]
    [Range(0.1f, 0.9f)]
    private float gatlingSpeedMultiplier = 0.75f;

    [SerializeField]
    [Range(0.1f, 0.9f)]
    private float gatlingMovespeedMultiplier = 0.5f;

    [SerializeField]
    private bool hasExplodingShots = false;

    [SerializeField]
    private Image shotgunImage;

    [SerializeField]
    private VoidEvent explosionEvent;

    float gunKnockBackAmount = 0, gunKnockBackTargetAmount = 0;

    [SerializeField]
    private bool forceFieldIsUp = false;
    [SerializeField]
    private float timeUntilForceFieldIsReady = 0, forceFieldCooldown;
    public GameObject forceFieldPrefab, forcefieldInstance;

    #endregion INSPECTOR

    void Awake()
    {

        moveDirection = Vector2.right;
        InitializePlayerStats(runtimeChoices.chosenHero); //Use the chosen stats to set baseline of this run.
        firePointoffset = firePoint.localPosition.x; // stored to be used to flip
    }

    private void Start()
    {

        _audioList = FindObjectOfType<AudioList>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        matDefault = spriteRenderer.material;

    }



    private void Update()
    {
        //Cheat to enable weapons for testing:
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hasShotgun = true;

            if (!hasGatlingGun)
            {
                hasGatlingGun = true;
                runtimePlayerStats.attackRate = runtimePlayerStats.attackRate * gatlingSpeedMultiplier;
                rangedCooldownMaxTime = runtimePlayerStats.attackRate;
            }



            hasExplodingShots = true;

            shotgunImage.gameObject.SetActive(true);
        }


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

        if (!forceFieldIsUp && timeUntilForceFieldIsReady > 0)
        {
            timeUntilForceFieldIsReady -= Time.deltaTime;
        }

        #endregion UpdateCooldowns


        #region MovementModifying
        if (rb != null)
        {
            rb.gravityScale = isHoldingJump ? baseGravity : dropGravityModifier;
        }

        #endregion MovementModifying



        #region UpdateSprites
        isMovingRight = moveDirection.x > 0;
        spriteRenderer.flipX = !isMovingRight;
        float fireDirection = Mathf.Sign(moveDirection.x);
        firePoint.transform.localPosition = new Vector3(firePointoffset * fireDirection, firePoint.localPosition.y, firePoint.localPosition.z);
        shotgunImage.rectTransform.rotation = isMovingRight ? Quaternion.Euler(new Vector3(0, 0, gunKnockBackAmount)) : Quaternion.Euler(new Vector3(0, -180, gunKnockBackAmount));
        gunKnockBackAmount = Mathf.Lerp(gunKnockBackAmount, gunKnockBackTargetAmount, Time.deltaTime * 20f);

        if (gunKnockBackTargetAmount > 0 && gunKnockBackTargetAmount < 20)
        {
            gunKnockBackTargetAmount -= 0.4f;
        }
        else if (gunKnockBackTargetAmount > 0)
        {
            gunKnockBackTargetAmount -= 2f;
        }

        #endregion UpdateSprites
    }


    public void MeleeAttack()
    {

    }

    public void ToggleForceField(bool active)
    {
        switch (active)
        {
            case true:
                forceFieldIsUp = true;
                audioList.forceFieldBegin.Play();
                audioList.OnForcefieldToggle(true);
                forcefieldInstance = Instantiate(forceFieldPrefab, transform.position + (((Vector3)moveDirection) * 5f), Quaternion.identity);
                if (!isMovingRight)
                {
                    Quaternion rotation = new Quaternion(transform.rotation.x, -55, transform.rotation.z, 1);
                    forcefieldInstance.transform.rotation = rotation;
                }
                forcefieldInstance.GetComponent<ForceField>().ForceFieldToggle(true);
                break;

            case false:
                timeUntilForceFieldIsReady = forceFieldCooldown;
                forceFieldIsUp = false;
                audioList.OnForcefieldToggle(false);
                forcefieldInstance.GetComponent<ForceField>().ForceFieldToggle(false);
                break;
        }
    }


    public void RangedAttack()
    {

        if (gunKnockBackTargetAmount < 30)
        {
            gunKnockBackTargetAmount += 30 - gunKnockBackTargetAmount;
        }
        else
        {
            gunKnockBackTargetAmount += 5;
        }


        List<Projectile> projectiles = new List<Projectile>();

        #region BaseLineAttack
        GameObject instance = Instantiate(projectile, firePoint.transform.position, Quaternion.identity);
        GameObject muzzle = Instantiate(muzzlePrefab, firePoint);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        rb.AddForce(moveDirection * projectileSpeed, ForceMode2D.Impulse);

        Projectile projInstance = instance.GetComponent<Projectile>();
        projectiles.Add(projInstance);
        projInstance.damage = (int)runtimePlayerStats.baseAttackDamage;
        Destroy(muzzle, 0.15f);

        #endregion BaseLineAttack

        audioList.PlayWithVariablePitch(audioList.attack1);

        #region ModifiedAttacks
        if (hasShotgun)
        {
            int magicNumberExtraProjectileCount = 2;

            float offsetMagicNumber = 1.5f;
            for (int i = 0; i < magicNumberExtraProjectileCount; i++)
            {

                int sign = i % 2 == 0 ? 1 : -1;
                float yPositionOfShot = sign * offsetMagicNumber;
                Vector3 shotPosition = new Vector3(0, yPositionOfShot, 0);

                GameObject shotgunInstance = Instantiate(projectile, firePoint.transform.position + (((Vector3)moveDirection) * -1) + shotPosition, Quaternion.identity);
                Rigidbody2D rbShotgun = shotgunInstance.GetComponent<Rigidbody2D>();
                rbShotgun.AddForce(moveDirection * projectileSpeed, ForceMode2D.Impulse);

                Projectile shotgunProjInstance = shotgunInstance.GetComponent<Projectile>();
                projectiles.Add(shotgunProjInstance);
                shotgunProjInstance.damage = (int)runtimePlayerStats.baseAttackDamage;


                if (i % 2 == 0 && i != 0)
                {
                    offsetMagicNumber += 1.5f;
                }
            }
        }

        if (hasGatlingGun)
        {
            //Doesn't do anything here.. -yet?
        }

        if (hasExplodingShots)
        {
            foreach (Projectile proj in projectiles)
            {
                proj.isExplodingProjectile = true;
            }
        }




        #endregion ModifiedAttacks

        justUsedRangedAttack = true;
    }

    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;
        playerHPEvent.Raise(currentHitPoints);
        if (currentHitPoints > 0) // Show damage effect
        {
            DamageAnimation();
            audioList.PlayWithVariablePitch(audioList.hurtHero);
        }
        else // Trigger death effect
        {
            DeathAnimation();
            if (!audioList.deathHero.isPlaying)
            audioList.PlayWithVariablePitch(audioList.deathHero);
        }

        if (floatingTextPrefab != null)
        {
            ShowFloatingText(damage); // Trigger floating text
            Debug.Log("Should trigger text");
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

    #endregion FloationText

    private bool CanPlayerAttack()
    {
        //Can have many more conditionals changing this in the future:
        if (!forceFieldIsUp)
        {
            return !justUsedRangedAttack;
        }
        return !forceFieldIsUp;
    }

    public void ReceiveInput(Player1Input input, float value)
    {
        switch (input)
        {
            case Player1Input.Horizontal:

                if (!forceFieldIsUp)
                {
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
                    if (IsPlayerCloseToObstacle((GetComponent<BoxCollider2D>().size.x / 2) + 1.4f))
                    {

                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }

                    if (hasGatlingGun && justUsedRangedAttack)
                    {
                        rb.velocity = new Vector2(rb.velocity.x * gatlingMovespeedMultiplier, rb.velocity.y);
                    }

                }

                break;
            case Player1Input.Attack:
                if (!CanPlayerAttack())
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
            case Player1Input.Jump:

                if (isGrounded && !forceFieldIsUp)
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
            case Player1Input.Explosion:
                explosionEvent.Raise();
                break;

            case Player1Input.StopForceField:
                if (forceFieldIsUp && timeUntilForceFieldIsReady <= 0)
                {
                    ToggleForceField(false);
                }
                break;

            case Player1Input.UseForceField:
                if (isGrounded && timeUntilForceFieldIsReady <= 0)
                {
                    if (forcefieldInstance == null)
                    {
                        rb.velocity = Vector2.zero;
                        ToggleForceField(true);
                    }
                }

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


            //Weapontypes addons:
            switch (playerItem.weaponType)
            {
                case PlayerItems.WeaponType.None:
                    break;

                case PlayerItems.WeaponType.ExplodingShots:
                    hasExplodingShots = true;
                    shotgunImage.sprite = playerItem.itemSprite;
                    shotgunImage.gameObject.SetActive(true);
                    break;

                case PlayerItems.WeaponType.Gatlinggun:
                    hasGatlingGun = true;
                    runtimePlayerStats.attackRate = runtimePlayerStats.attackRate * gatlingSpeedMultiplier;
                    rangedCooldownMaxTime = runtimePlayerStats.attackRate;
                    shotgunImage.sprite = playerItem.itemSprite;
                    shotgunImage.gameObject.SetActive(true);
                    break;

                case PlayerItems.WeaponType.Shotgun:
                    hasShotgun = true;
                    shotgunImage.sprite = playerItem.itemSprite;
                    shotgunImage.gameObject.SetActive(true);
                    break;
            }
        }

        playerItems.AddRange(chosenItems.Except(playerItems)); //Add new items to playeritems
        Debug.Log("Updated runtime playerstats with new item!");

        //Strictly speaking only necessary if playerHP actually changed here, but for good measure:
        playerHPEvent.Raise(currentHitPoints);
    }

    public void InitializePlayerStats(P1Stats baselineStats)
    {
        runtimePlayerStats.baseAttackDamage = baselineStats.baseAttackDamage;
        runtimePlayerStats.maxHitPoints = baselineStats.maxHitPoints;
        runtimePlayerStats.moveSpeed = baselineStats.moveSpeed;

        runtimePlayerStats.attackRate = baselineStats.attackRate;
        rangedCooldownMaxTime = runtimePlayerStats.attackRate;

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

        BoxCollider2D boxColl2D = GetComponent<BoxCollider2D>();
        boxColl2D.offset = runtimeChoices.chosenHero.colliderOffset;
        boxColl2D.size = runtimeChoices.chosenHero.colliderSize;

        currentHitPoints = baselineStats.startingHitPoints;
        playerHPEvent.Raise(currentHitPoints);
        //healthBar.VisualiseHealthChange(baselineStats.startingHitPoints);

        runtimeChoices.baselineItem = baselineStats.startItem;

        //Weapontypes addons:
        switch (baselineStats.startItem.weaponType)
        {
            case PlayerItems.WeaponType.None:
                //Do nothing
                break;

            case PlayerItems.WeaponType.ExplodingShots:
                hasExplodingShots = true;
                shotgunImage.sprite = baselineStats.startItem.itemSprite;
                shotgunImage.gameObject.SetActive(true);
                break;

            case PlayerItems.WeaponType.Gatlinggun:
                hasGatlingGun = true;
                runtimePlayerStats.attackRate = runtimePlayerStats.attackRate * gatlingSpeedMultiplier;
                rangedCooldownMaxTime = runtimePlayerStats.attackRate;
                shotgunImage.sprite = baselineStats.startItem.itemSprite;
                shotgunImage.gameObject.SetActive(true);
                break;

            case PlayerItems.WeaponType.Shotgun:
                hasShotgun = true;
                shotgunImage.sprite = baselineStats.startItem.itemSprite;
                shotgunImage.gameObject.SetActive(true);
                break;
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

        Destroy(instance.gameObject, instance.duration);
        Invoke("DeathExplode", 1);
    }

    public void DeathExplode() // Add Particle Burst
    {
        audioList.explosion.Play();
        audioList.deathHero.Play();
        shotgunImage.gameObject.SetActive(false);
        ParticleSystem instance = Instantiate(deathExplosion, particlePoint.position, particlePoint.rotation);
        CameraShake camshake = FindObjectOfType<CameraShake>();
        camshake.StartShake(camshake.shakePropertyOnMinionDie);
        Destroy(gameObject);
        Destroy(instance.gameObject, instance.duration);
    }

    public void RestoreToFullHealth()
    {
        currentHitPoints = runtimePlayerStats.maxHitPoints;
        playerHPEvent.Raise(currentHitPoints);
    }
}
