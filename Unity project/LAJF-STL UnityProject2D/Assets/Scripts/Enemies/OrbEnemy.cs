using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbEnemy : EnemyBehaviour
{
    [Header("Prefabs and settings")]
    public Orb orbPrefab;

    [Header("Runtime-changing fields to track")]
    public Orb orb;
    public Transform currentOrbTransform;
    public Transform currentOrbTargetTransform;

    [Range(0.1f, 2f)]
    [SerializeField]
    private float orbMovementWaitIntervals;

    [Range(0.5f, 20f)]
    [SerializeField]
    private float orbMovementVelocity;

    private float orbMovementTimer = 0;
    private bool orbIsMovingToNextPosition;

    [SerializeField]
    private Transform[] orbPositions; //orbPositions[0] = initial position. Assign in inspector.



    public override void InitalizeEnemy()
    {
        base.InitalizeEnemy();

        Orb orbInstance = Instantiate(orbPrefab, orbPositions[0].position, Quaternion.identity, transform);
        currentOrbTransform = orbPositions[0];
        currentOrbTargetTransform = orbPositions[1];
        //Instantiate orb prefab
    }



    protected override void MoveToTarget()
    {
        base.MoveToTarget();

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
            Vector3 newOrbPosition = Vector3.MoveTowards(orb.transform.position, currentOrbTargetTransform.position, orbMovementVelocity * Time.deltaTime);
            orb.transform.position = newOrbPosition;

            orbIsMovingToNextPosition = Vector3.Distance(orb.transform.position, currentOrbTargetTransform.position) < 0.1f ? false : true;
        }




        #endregion OrbMovement

        Debug.Log("Frog hop");
    }

    protected override void RangedAttack()
    {
        base.RangedAttack();
        Debug.Log("Im a child of enemy behavior");
    }


}