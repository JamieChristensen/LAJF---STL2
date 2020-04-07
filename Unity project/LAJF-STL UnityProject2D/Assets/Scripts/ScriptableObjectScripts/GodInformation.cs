using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "GodInformation", menuName = "ScriptableObject/GodInformation")]
public class GodInformation : ScriptableObject
{
    public string godName;
    public Sprite selectionIcon;
    public Sprite topBarIcon;

    //TODO: Add attack-sprites/effects?
    //TODO: Add emotes?
    //TODO: 
}
