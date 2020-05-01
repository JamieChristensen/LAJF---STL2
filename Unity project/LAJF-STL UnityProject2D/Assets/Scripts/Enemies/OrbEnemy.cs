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

    private int currentHealth;

    private Transform playerTransform;

    void Start()
    {
        InitalizeEnemy();
        playerTransform = FindObjectOfType<P1Controller>().transform;
    }


    public override void InitalizeEnemy()
    {
        currentHealth = 10;

        Orb orbInstance = Instantiate(orbPrefab, transform.position, Quaternion.identity, transform).GetComponent<Orb>();

        currentOrbTransform = orbPositions[0];
        currentTargetTransformIndex = 1;
        currentOrbTargetTransform = orbPositions[currentTargetTransformIndex];

        orb = orbInstance;

        base.InitalizeEnemy();

        currentHealth = 10;
        //Instantiate orb prefab
    }

    public override void TakeDamage(int damage)
    {
        if (!isOrbDead)
        {
            return;
        }

        base.TakeDamage(damage);
    }

    protected override void MoveToTarget()
    {
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






    protected override void Die()
    {
        Debug.Log("No.");
    }

    protected override void RangedAttack()
    {
        base.RangedAttack();
        Debug.Log("Im a child of enemy behavior");


    }


}