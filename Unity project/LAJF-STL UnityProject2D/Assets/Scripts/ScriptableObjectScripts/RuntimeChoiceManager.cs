using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuntimeChoiceManager", menuName = "ScriptableObject/Manager/RuntimeChoiceManager")]
public class RuntimeChoiceManager : ScriptableObject
{
    public ChoiceCategory runtimeChoices;

    public void ResetRun()
    {
        runtimeChoices.runTimeLoopCount = 1;
        runtimeChoices.enemyModifiers = new List<EnemyModifier>();
        runtimeChoices.chosenHero = null;
        runtimeChoices.chosenGods = new GodInformation[3];
        runtimeChoices.chosenEnvironments = new Environment[4];
        runtimeChoices.enemies = new List<Enemy>();
        runtimeChoices.enemyModifiers = new List<EnemyModifier>();
        runtimeChoices.playerItems = new List<PlayerItems>();
    }

    public void SetLoopCount(int runtimeLoopCount)
    {
        if (runtimeLoopCount > 0 && runtimeLoopCount < 5)
        {
            runtimeChoices.runTimeLoopCount = runtimeLoopCount;
        }
    }

    public void ResetCharacter()
    {
        runtimeChoices.chosenHero = null;
    }
    public void ResetGods()
    {
        runtimeChoices.chosenGods = new GodInformation[3];
    }
    public void ResetTheme()
    {
        runtimeChoices.chosenEnvironments = new Environment[3];
    }
    public void ResetMinions()
    {
        runtimeChoices.enemies = new List<Enemy>();
    }
    public void ResetModifiers()
    {
        runtimeChoices.enemyModifiers = new List<EnemyModifier>();
    }

    public void ResetItems()
    {
        runtimeChoices.playerItems = new List<PlayerItems>();
    }

}
