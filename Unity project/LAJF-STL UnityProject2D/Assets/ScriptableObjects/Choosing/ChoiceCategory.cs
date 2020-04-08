using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu (fileName = "New Choice Category", menuName = "ScriptableObject/ChoiceCategory")]
public class ChoiceCategory : ScriptableObject
{
    public string CategoryName;
    public GameObject[] Options;

    public GodInformation[] chosenGods = new GodInformation[3];
    public List<PlayerItems> playerItems;
    public List<Enemy> enemies;
    public List<EnemyModifier> enemyModifiers;
    public Environment[] chosenEnvironments;

    [Header("Runtime Only")]
    public int runTimeLoopCount = 1;
    [Header("Pre-phase")]

    public P1Stats chosenHero; //Using this instead of character game-object.
    public GameObject character;
    public GameObject theme;
    [Header("First Loop")]
    public Opponent firstOpponent;
    public GameObject firstItem;
    [Header("Second Loop")]
    public Opponent secondOpponent;
    public GameObject secondItem;
    [Header("Third Loop")]
    public Opponent thirdOpponent;
    public GameObject thirdItem;
    [Header("Boss Fight")]
    public Opponent fourthOpponent;

}

[Serializable]
public class Opponent
{
    public GameObject minion;
    public GameObject modifier;
}



