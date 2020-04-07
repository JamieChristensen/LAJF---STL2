using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Modifier", menuName = "ScriptableObject/Modifier for enemy")]
public class EnemyModifier : ScriptableObject
{

    [Tooltip("Remember to use _ before or after the text")]
    public new string name;
    public string description;

    public Sprite sprite;
    // modifiers 
        
     
}
