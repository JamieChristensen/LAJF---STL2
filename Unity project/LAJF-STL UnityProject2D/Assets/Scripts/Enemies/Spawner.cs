﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class Spawner : MonoBehaviour
{
    //Spawns chest, enemy, perhaps more in the future.
    [Header("EnemyPrefabs")]
    public GameObject enemyPrefab;
    public GameObject agileEnemyPrefab;
    public GameObject orbEnemyPrefab;
    public GameObject splitterEnemyPrefab;

    public Enemy agileEnemy, orbEnemy, splitterEnemy;
    public EnemyModifier blessed, shoulderConnon, angry;

    [Header("Other")]
    public GreatChest greatChest;
    public ChoiceCategory runtimeChoices;

    public Vector3 enemySpawnPos;

    public Vector3 chestPosition;

    public VoidEvent monsterSpawnedEvent;
    public void Start()
    {
        greatChest = FindObjectOfType<GreatChest>();
        chestPosition = greatChest.transform.position;
    }

    public void ResetChestState()
    {
        greatChest.SetInactivePosition(chestPosition);
        greatChest.ReInitialize();
    }

    public void SpawnEnemy()
    {
        switch (runtimeChoices.runTimeLoopCount)
        {
            case 1:
                StartCoroutine(SpawnEnemyAfterDelay(3));
                break;

            case 2:
                StartCoroutine(SpawnEnemyAfterDelay(3));
                break;

            case 3:
                StartCoroutine(SpawnEnemyAfterDelay(3));
                break;

            case 4:
                StartCoroutine(SpawnEnemyAfterDelay(5));
                break;

            default:
                StartCoroutine(SpawnEnemyAfterDelay(5));
                break;
        }


    }

    public void SpawnRandomEnemy()
    {
        int random1 = Random.Range(1, 4);
        int random2 = Random.Range(1, 4);
        
        runtimeChoices.runTimeLoopCount = random1;
        runtimeChoices.enemies = new List<Enemy>();
        runtimeChoices.enemies.Add(agileEnemy);
        runtimeChoices.enemies.Add(orbEnemy);
        runtimeChoices.enemies.Add(splitterEnemy);
        runtimeChoices.enemyModifiers = new List<EnemyModifier>();
        switch (random2)
        {
            case 1:
                runtimeChoices.enemyModifiers.Add(blessed);
                runtimeChoices.enemyModifiers.Add(blessed);
                runtimeChoices.enemyModifiers.Add(blessed);
                break;

            case 2:
                runtimeChoices.enemyModifiers.Add(shoulderConnon);
                runtimeChoices.enemyModifiers.Add(shoulderConnon);
                runtimeChoices.enemyModifiers.Add(shoulderConnon);
                break;

            case 3:
                runtimeChoices.enemyModifiers.Add(angry);
                runtimeChoices.enemyModifiers.Add(angry);
                runtimeChoices.enemyModifiers.Add(angry);
                break;
        }

        SpawnEnemy();
    }


    IEnumerator SpawnEnemyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int runTimeLoopCount = runtimeChoices.runTimeLoopCount;
        Enemy enemy = runtimeChoices.enemies[runTimeLoopCount - 1];
        Enemy.EnemyType enemyType = enemy.enemyType;

        Vector3 deltaVector = new Vector3(Random.Range(0, 60), 0, 0);

        GameObject go;
        EnemyBehaviour enemyBehaviour;

        switch (enemyType)
        {
            case Enemy.EnemyType.None:
                go = Instantiate(enemyPrefab, enemySpawnPos - deltaVector, Quaternion.identity);
                enemyBehaviour = go.GetComponent<EnemyBehaviour>();
                break;

            case Enemy.EnemyType.Agile:
                go = Instantiate(agileEnemyPrefab, enemySpawnPos - deltaVector, Quaternion.identity);
                enemyBehaviour = go.GetComponent<AgileEnemy>();
                break;

            case Enemy.EnemyType.Orb:
                go = Instantiate(orbEnemyPrefab, enemySpawnPos - deltaVector, Quaternion.identity);
                enemyBehaviour = go.GetComponent<OrbEnemy>();
                break;

            case Enemy.EnemyType.Splitter:
                //TODO: Make splitter:
                go = Instantiate(splitterEnemyPrefab, enemySpawnPos - deltaVector, Quaternion.identity);
                enemyBehaviour = go.GetComponent<SplitterEnemy>();
                break;

            default:
                //Default is only here to ensure that enemyBehaviour is always assigned for future use in this method call.
                go = Instantiate(enemyPrefab, enemySpawnPos - deltaVector, Quaternion.identity);
                enemyBehaviour = go.GetComponent<EnemyBehaviour>();
                break;
        }


        EnemyModifier[] modifiers = new EnemyModifier[] { runtimeChoices.enemyModifiers[runTimeLoopCount - 1] };
        enemyBehaviour.InitalizeEnemy(enemy, modifiers);        //take account for boss-amount of modifiers

        monsterSpawnedEvent.Raise();

    }
}
