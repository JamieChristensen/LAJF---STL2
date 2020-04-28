using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileEnemy : EnemyBehaviour
{
    protected override void MoveToTarget()
    {
        base.MoveToTarget();
        Debug.Log("Frog hop");
    }

    protected override void RangedAttack()
    {
        base.RangedAttack();
        Debug.Log("Im a child of enemy behavior");
    }

}
