using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName = "ScriptableObject/Enemy")]
public class Enemy : ScriptableObject
{
    // generic
    public new string name;
    public int health;
    public float speed;

    // visual
    public Sprite sprite;
    public float scaleFactor; 

    // combat 
    public int damage;
    public string attackType;
    public int range;
    public float attackSpeed;
    // just for testing

    public string GenerateName(List<EnemyModifier> modifiers)
    {
        string modifiedName = name;

        if(modifiers.Count > 0)
        {
            foreach(EnemyModifier m in modifiers)
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
