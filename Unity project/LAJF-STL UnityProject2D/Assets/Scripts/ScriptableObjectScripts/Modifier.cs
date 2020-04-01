using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Modifier", menuName = "ScriptableObject/Modifier")]
public class Modifier : ScriptableObject
{

    /// <summary>
    /// 
    /// </summary>
    [Tooltip("Remember to use _ before or after the text")]
    public string text;
    public string description;

    public Sprite art;
    // modifiers 
        
     
}
