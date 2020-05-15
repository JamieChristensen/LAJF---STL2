using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "GodInformation", menuName = "ScriptableObject/GodInformation")]
public class GodInformation : ScriptableObject
{
    public string godName;
    public string description;
    public Sprite selectionIcon;
    public Sprite topBarIcon;
    public Sprite whenShooting;
    public Sprite whenCharging;

    //TODO: Add attack-sprites/effects?
    public enum AttackTypes
    {
        Lightning,
        Fireball,
        Laserbeam
    }
    public AttackTypes attackType;
    //TODO: Add emotes?
    //TODO: 
}
