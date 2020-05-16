using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbEnemy : EnemyBehaviour
{
    [Header("Prefabs and settings")]
    public GameObject orbPrefab;

    [Header("Runtime-changing fields to track")]
    public Orb orb;
    public Transform currentOrbTransform;
    public Transform currentOrbTargetTransform;

    [Range(0.1f, 2f)]
    [SerializeField]
    private float orbMovementWaitIntervals;

    [Range(0.2f, 3f)]
    [SerializeField]
    private float orbAttackCooldownTime;

    private float orbAttackTimer;

    [Range(0.5f, 20f)]
    [SerializeField]
    private float orbMovementVelocity;

    private float orbMovementTimer = 0;
    [SerializeField]
    private bool orbIsMovingToNextPosition;

    [SerializeField]
    private Transform[] orbPositions; //orbPositions[0] = initial position. Assign in inspector.

    public bool isOrbDead = false;

    private int currentTargetTransformIndex = 1;


    private Transform playerTransform;

    public bool isInitialized = false;
    public Rigidbody2D rb, rb2;

    public Material spriteMat;

    public Transform orbTransformRoot;

    void Start()
    {
        playerTransform = FindObjectOfType<P1Controller>().transform;
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rb2 = GetComponent<Rigidbody2D>();
        matDefault = spriteRenderer.material;


    }


    public override void InitalizeEnemy()
    {
        Orb orbInstance = Instantiate(orbPrefab, transform.position, Quaternion.identity, transform).GetComponent<Orb>();


        currentOrbTransform = orbPositions[0];
        currentTargetTransformIndex = 1;
        currentOrbTargetTransform = orbPositions[currentTargetTransformIndex];

        orb = orbInstance;

        base.InitalizeEnemy();
        //Instantiate orb prefab
    }

    public override void TakeDamage(int damage)
    {
        if (!isOrbDead)
        {
            ShowFloatingText();
            return;
        }

        base.TakeDamage(damage);
    }

    #region FloatingText

    public void ShowFloatingText()
    {
        if (floatingCanvasParent == null) // if the canvas is not yet instantiated
        {
            floatingCanvasParent = GameObject.Find("FloatingCanvas"); // find the canvas (parent) for text
        }

        float randomX = UnityEngine.Random.Range(-4.5f, 4.5f); // Random position.x
        float randomY = UnityEngine.Random.Range(-1.5f, 4.5f); // Random position.y
        Vector3 randomVector = new Vector3(randomX, randomY, 0); // Random combined position
        floatingTextInstance = Instantiate(floatingTextPrefab, transform.position + randomVector, Quaternion.identity, floatingCanvasParent.transform); // instantiate text object

        floatingTextInstance.GetComponent<FloatingTextEffects>().setText("INVULNERABLE!"); //Sets the text of the text object - the rest will happen in the instance (FloatingTextEffects.cs)
    }


    #endregion FloatingText

    protected override void MoveToTarget()
    {
        if (!isInitialized)
        {
            InitalizeEnemy();
            isInitialized = true;
        }

        if (flipped)
        {
            orbTransformRoot.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            orbTransformRoot.localRotation = Quaternion.Euler(0, 180, 0);
        }

        base.MoveToTarget();
        
        if (isOrbDead)
        {
            return;
        }
        #region OrbMovement
        //Increment timer:




        if (!orbIsMovingToNextPosition)
        {
            orbMovementTimer += Time.deltaTime;
        }

        if (orbMovementTimer >= orbMovementWaitIntervals)
        {
            orbIsMovingToNextPosition = true;
            orbMovementTimer = 0;
        }

        //Update orb positions.
        if (orbIsMovingToNextPosition)
        {
            Vector3 localOrbTarget = currentOrbTargetTransform.position;
            Vector3 newOrbPosition = Vector3.MoveTowards(orb.transform.position, localOrbTarget, orbMovementVelocity * Time.deltaTime);
            orb.transform.position = newOrbPosition;


            if (Vector3.Distance(orb.transform.position, localOrbTarget) < 0.1f)
            {
                currentOrbTransform = currentOrbTargetTransform;
                currentTargetTransformIndex += 1;
                currentTargetTransformIndex %= orbPositions.Length;
                currentOrbTargetTransform = orbPositions[currentTargetTransformIndex];
                orbIsMovingToNextPosition = false;

                orb.Attack(playerTransform);
            }
        }




        #endregion OrbMovement



    }


    protected override void RangedAttack()
    {
        base.RangedAttack();
        Debug.Log("Im a child of enemy behavior");


    }


}