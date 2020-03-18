using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName = "ScriptableObject/Enemy")]
public class Enemy : ScriptableObject
{
    public new string name;

    public Sprite sprite;

    public int health;
    public int damage;
    public int speed;

    //public Trait trait;
    // OG stats
    // public int length;
    // public int width;

}
