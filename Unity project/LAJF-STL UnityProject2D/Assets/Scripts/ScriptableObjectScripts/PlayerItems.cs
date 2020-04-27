using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerItem", menuName = "ScriptableObject/Player1/Item")]
public class PlayerItems : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite itemSprite;

    public int healthModifier;
    public float speedModifier;

    public float damageModifier;

    public enum WeaponType
    {
        None, Shotgun, Gatlinggun, ExplodingShots
    };

    public WeaponType weaponType;

}
