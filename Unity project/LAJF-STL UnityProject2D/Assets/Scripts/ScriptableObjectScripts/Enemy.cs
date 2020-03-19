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
    public int attackSpeed;
    
    // can be called traits that is made
    public object[] traits; 



    //public Trait trait;
    // OG stats
    // public int length;
    // public int width;

}
