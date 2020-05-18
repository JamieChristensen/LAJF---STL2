using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterEnemy : EnemyBehaviour
{
    [Header("Splitter-specifics")]
    public bool isSplitted;

    [Range(1, 3)]
    public int amountOfSplits = 2;

    public GameObject splittedEnemy;

    [SerializeField]
    private Sprite splittedEnemySprite; //Assign through scriptableObject, unless sprite is preset in prefab. 

    [SerializeField]
    private bool canMove = true;

    private float canMoveTimer = 0;
    [SerializeField]
    private float canMoveMaxTime = 2;

    [Tooltip("Applied to upwards and sideways forces when the smaller splitters are spawned")]
    [SerializeField]
    private float spawnForceModifier;



    protected override void MoveToTarget()
    {
        if (canMoveTimer >= canMoveMaxTime)
        {
            canMove = true;
        }

        if (!canMove)
        {
            canMoveTimer += Time.deltaTime;
        }
        else
        {
            base.MoveToTarget();
        }

    }

    protected override void RangedAttack()
    {
        base.RangedAttack();

    }

    public override void InitalizeEnemy()
    {
        List<EnemyModifier> enemyModifiers = new List<EnemyModifier>();
        agent = runtimeChoices.enemies[runtimeChoices.enemies.Count - 1];
        modifiers.Add(runtimeChoices.enemyModifiers[runtimeChoices.enemyModifiers.Count - 1]);


        //TODO: Modify this sprite so that splitter and splitted isn't identical.
        spriteRenderer.sprite = agent.sprite;


        name = agent.GenerateName(modifiers);
        nameUI.SetText(name);
        currentHealth = agent.health;
        maxHealth = currentHealth;
        healthBar.UpdateHPValues(currentHealth, maxHealth);
        StartCoroutine(UINameFadeOut(3));
    }

    protected override void Die()
    {
        if (!(currentHealth <= 0))
        {
            return;
        }


        if (!isSplitted)
        {
            float horizontalForceModifier = Random.Range(0.5f, 1f) * spawnForceModifier;
            float verticalForceModifier = Random.Range(1f, 2f) * spawnForceModifier;

            float distanceModifier = 3;

            //Instantiate smaller clones.
            for (int i = 0; i < amountOfSplits; i++)
            {
                SplitterEnemy instance = Instantiate(splittedEnemy, transform.position + (Vector3.right * (Random.Range(0, 1) * 2 - 1)), Quaternion.identity).GetComponent<SplitterEnemy>();
                instance.isSplitted = true;
                instance.canMove = false;
                Rigidbody2D instanceRigidbody = instance.GetComponent<Rigidbody2D>();
                instanceRigidbody.AddForce(Vector2.up * horizontalForceModifier, ForceMode2D.Impulse);
                instanceRigidbody.AddForce(Vector2.left * verticalForceModifier, ForceMode2D.Impulse);

                instance.InitalizeEnemy();

                instance.desiredDistance = desiredDistance + distanceModifier;

                instance.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                instance.currentHealth = agent.health / 2;
                verticalForceModifier *= -1; //ensure they fly in separate directions
                distanceModifier *= -1; //Really makes this script only work for two clones, but we only ever need that anyway.
            }
            isSplitted = true;
        }

        if (isSplitted)
        {
            base.Die();
        }
    }
}
