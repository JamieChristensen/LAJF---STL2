using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObject/Enemy")]
public class Enemy : ScriptableObject
{

    public enum EnemyType
    {
        None, Agile, Orb, Splitter
    }

    // generic
    public new string name;
    public string description; //Could be used for tooltips like similar scriptableObjects
    public int health;
    public float speed;

    // visual
    [Space]
    [Header("Default sprite and texture")]
    public Sprite sprite; //Default
    public Texture2D texture;
    [Space]
    [Header("Ramp-up-sprite")]
    public Sprite rampUpForAttackSprite;
    [Space]
    [Header("Attack-sprite")]
    public Sprite attackingSprite;


    public float scaleFactor;

    // combat 
    public int damage;
    public string attackType;
    public int range;
    public float attackSpeed;
    // just for testing
    public string aOrAn;

    [Header("EnemyType")]
    public EnemyType enemyType;
    public string GenerateName(List<EnemyModifier> modifiers)
    {
        string modifiedName = name;

        if (modifiers.Count > 0)
        {
            foreach (EnemyModifier m in modifiers)
            {
                // look for _ 
                string extraText = m.name.Trim();

                if (extraText.IndexOf('_') == 0)
                    modifiedName += " " + extraText.Split('_')[1];
                else
                {
                    modifiedName = extraText.Split('_')[0] + " " + modifiedName;
                }

            }
        }
        return modifiedName;
    }



    //public Trait trait;
    // OG stats
    // public int length;
    // public int width;

}
