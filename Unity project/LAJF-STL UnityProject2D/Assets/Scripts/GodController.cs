using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GodController : MonoBehaviour
{
    [SerializeField]
    private ChoiceCategory runtimeChoices;
    [SerializeField]
    private GodInformation godInfo;

    [SerializeField]
    [Range(1, 3)]
    private int godNumber;

    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private bool isEnabled;

    public void Start()
    {
        if (runtimeChoices.chosenGods.Length >= godNumber)
        {
            godInfo = runtimeChoices.chosenGods[godNumber];
            sprite = godInfo.topBarIcon;
            spriteRenderer.sprite = sprite;
            isEnabled = true;
        }
        else
        {
            spriteRenderer.sprite = null;
            isEnabled = false;
        }

    }



    public int GetGodNumber()
    {
        return godNumber;
    }

}