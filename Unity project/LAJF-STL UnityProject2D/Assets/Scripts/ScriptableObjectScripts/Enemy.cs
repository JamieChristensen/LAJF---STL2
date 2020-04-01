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

    public string generateName(List<Modifier> modifiers)
    {
            string modifiedName = name;

        if(modifiers.Count > 0)
        {
            foreach(Modifier m in modifiers)
            {
                // look for _ 
                string extraText = m.text.Trim();

                if (m.text.IndexOf('_') == 0)
                    modifiedName += m.text.Split('_')[1];
                else
                {
                    modifiedName = m.text.Split('_')[0] + " " + modifiedName;
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
