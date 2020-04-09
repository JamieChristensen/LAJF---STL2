using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Characterpool", menuName = "ScriptableObject/Pool/Characterpool")]
public class CharacterPool : ScriptableObject
{
    public List<P1Stats> characterStats;
}
