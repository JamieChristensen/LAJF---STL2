using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier", menuName = "ScriptableObject/Modifier for enemy")]
public class EnemyModifier : ScriptableObject
{
    [Tooltip("Remember to use _ before or after the text")]
    public new string name;
    public string description;

    public ModifierType modifier;


    public Sprite sprite;
    // modifiers 

    [Header("Modifiers")]
    [Header("ShoulderCannon")]
    public GameObject shoulderCannon;
    
    [Header("Blessed / Revive")]
    [ColorUsage(true, true)]
    Color holyOutline;
    public GameObject holyHightlight;

    [Header("Angry")]
    public Sprite angrySprite;


    public enum ModifierType
    {
        Angry, ShoulderCannon, Blessed
    };

    //TODO: Implement modifier functionality. (ANGRY + BLESSED + SHOULDER CANNON)
}
