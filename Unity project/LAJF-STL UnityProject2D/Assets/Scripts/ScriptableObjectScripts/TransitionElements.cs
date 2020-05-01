using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (fileName = "New Transition Elements", menuName = "ScriptableObject/UI/Transition Elements")]
public class TransitionElements : ScriptableObject
{
    public int sceneIndex;
    public string details;
    public string[] introFillers, heroComments;
    public bool startTransparent;
    public TextElement[] textElement;

    public List<Sprite> transitionBackgrounds, environmentBackgrounds;
    public ChoiceCategory runtimeChoices;



    public void GetEnvironmentBackgroundsForTransition()
    {
        environmentBackgrounds = new List<Sprite>();
        foreach (Environment environment in runtimeChoices.chosenEnvironments)
        {
            environmentBackgrounds.Add(environment.environmentSprite);
        }
    }


}

[Serializable] public class TextElement
{
    //public string textInput;
    public string[] textInputs;
    public float timeOfTextDisplayed;
}
