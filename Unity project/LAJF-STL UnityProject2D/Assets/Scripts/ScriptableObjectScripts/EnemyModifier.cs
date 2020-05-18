using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier", menuName = "ScriptableObject/Modifier for enemy")]
public class EnemyModifier : ScriptableObject
{
    public AudioClip nameClip, representationClip;
    [Tooltip("Remember to use _ before or after the text")]
    public new string name;
    public string description;

    public ModifierType modifierType;


    public Sprite sprite;
    // modifiers 

    [Header("Modifiers")]
    [Header("ShoulderCannon")]
    public Sprite shoulderCannonVisual;
    public int damage;
    public GameObject projectilePrefab;

    [Header("Blessed / Revive")]
    [Space]

    [ColorUsage(true, true)]
    public Color holyOutline;
    public GameObject holyHightlight;

    [Header("Angry")]
    public Sprite angrySprite;


    public enum ModifierType
    {
        None, Angry, ShoulderCannon, Blessed
    };

    //TODO: Implement modifier functionality. (ANGRY + BLESSED + SHOULDER CANNON)

    //TODO: Shoulder cannon applied in enemyBehaviour. (As a baseline - make "usemodifier"-function that is applied in start of all updates and perhaps other checks for dying and such.)
    //TODO: Apply modifier visuals when most convenient in gameobjects lifecycle.
    public void FireShoulderCannon(Vector2 velocity, Vector3 position, int damage)
    {
        GameObject go = Instantiate(projectilePrefab, position, Quaternion.identity);
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
        rb.gravityScale = 1;
        go.GetComponent<Projectile>().damage = damage;
    }

    public void ApplyShoulderCannonVisuals(EnemyBehaviour enemy)
    {
        enemy.shoulderCannonRenderer.sprite = shoulderCannonVisual;
    }

    public void ApplyHolyVisuals(EnemyBehaviour enemyToApplyTo)
    {
        Instantiate(holyHightlight, enemyToApplyTo.transform.position, Quaternion.identity, enemyToApplyTo.transform);
        enemyToApplyTo.outline = holyOutline;
        enemyToApplyTo.outlineThiccness = 0.02f;
    }

    public void Revive(EnemyBehaviour enemyToApplyTo)
    {
        FindObjectOfType<AudioList>().resurrection.Play();
        Enemy agent = enemyToApplyTo.agent;
        Enemy.EnemyType enemyType = agent.enemyType;
        Spawner spawner = FindObjectOfType<Spawner>();

        GameObject go;
        switch (enemyType)
        {
            case Enemy.EnemyType.Agile:
                go = Instantiate(spawner.agileEnemyPrefab, enemyToApplyTo.transform.position, Quaternion.identity);
                break;
            case Enemy.EnemyType.Orb:
                go = Instantiate(spawner.orbEnemyPrefab, enemyToApplyTo.transform.position, Quaternion.identity);
                break;
            case Enemy.EnemyType.Splitter:
                go = Instantiate(spawner.splitterEnemyPrefab, enemyToApplyTo.transform.position, Quaternion.identity);
                break;
            default:
                go = Instantiate(spawner.enemyPrefab, enemyToApplyTo.transform.position, Quaternion.identity);
                break;
        }

        EnemyBehaviour newEnemy = go.GetComponent<EnemyBehaviour>();

        newEnemy.name = name;
        newEnemy.spriteRenderer.sprite = agent.sprite;
        newEnemy.nameUI.SetText(enemyToApplyTo.name);

        newEnemy.nameUI = enemyToApplyTo.nameUI;

        newEnemy.currentHealth = agent.health;
        newEnemy.maxHealth = newEnemy.currentHealth;
        newEnemy.healthBar.UpdateHPValues(newEnemy.currentHealth, newEnemy.maxHealth);

        EnemyModifier[] enemyModifiers = new EnemyModifier[0];

        newEnemy.InitalizeEnemy(agent, enemyModifiers);
    }

    public void ApplyAngrySprite(EnemyBehaviour enemy)
    {
        enemy.angrySpriteRenderer.sprite = angrySprite;
    }
}
