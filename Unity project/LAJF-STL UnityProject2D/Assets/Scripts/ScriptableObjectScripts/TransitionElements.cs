using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (fileName = "New Transition Elements", menuName = "ScriptableObject/UI/Transition Elements")]
public class TransitionElements : ScriptableObject
{
    public int sceneIndex;
    public string details;
    public bool startTransparent;
    public TextElement[] textElement;

}

[Serializable] public class TextElement
{
    public string textInput;
    public string[] textInputs;
    public float timeOfTextDisplayed;
}
