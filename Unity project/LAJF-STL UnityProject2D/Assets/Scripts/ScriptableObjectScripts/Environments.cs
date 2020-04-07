using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Environments", menuName = "ScriptableObject/Environments")]
public class Environments : ScriptableObject
{

    public string environmentName;
    public string description;
    public Sprite environmentSprite;

    public int environmentIndices;

}

