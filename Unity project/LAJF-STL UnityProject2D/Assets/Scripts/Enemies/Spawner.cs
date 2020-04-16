using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Spawns chest, enemy, perhaps more in the future.
    public GameObject enemyPrefab;

    public GreatChest greatChest;
    public ChoiceCategory runtimeChoices;

    public Vector3 enemySpawnPos;

    public Vector3 chestPosition;


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

        Vector3 deltaVector = new Vector3(Random.Range(0,60), 0, 0);
        GameObject go = Instantiate(enemyPrefab, enemySpawnPos-deltaVector, Quaternion.identity);
        EnemyBehaviour enemyBehaviour = go.GetComponent<EnemyBehaviour>();
        //Runtimeloopcount incremented once per item-choice.
        int runTimeLoopCount = runtimeChoices.runTimeLoopCount;

        Enemy enemy = runtimeChoices.enemies[runTimeLoopCount-1];
        EnemyModifier[] modifiers = new EnemyModifier[] { runtimeChoices.enemyModifiers[runTimeLoopCount-1] };
        enemyBehaviour.InitalizeEnemy(enemy, modifiers);

        //take account for boss-amount of modifiers
    }
}
