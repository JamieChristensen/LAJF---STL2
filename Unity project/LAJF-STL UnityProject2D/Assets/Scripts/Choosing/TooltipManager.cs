using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public ChooseBetweenOptionsGiven choiceManager;
    public p1Choose player1Selector;
    public p2Choose player2Selector;

    public string tooltipText = "";

    private string choiceType;
    private int choiceIndex;

    //Choice arrays from choicemanaging script:
    private P1Stats[] p1Stats;
    private PlayerItems[] playerItemChoices;
    private EnemyModifier[] enemyModifierChoices;

    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        choiceIndex = -1; //Initial none-chosen.
        if (choiceManager == null)
        {
            choiceManager = FindObjectOfType<ChooseBetweenOptionsGiven>();
        }
        if (player1Selector == null)
        {
            player1Selector = FindObjectOfType<p1Choose>();
        }
        if (player2Selector == null)
        {
            player2Selector = FindObjectOfType<p2Choose>();
        }

        choiceType = choiceManager.choiceType;

        switch (choiceType)
        {
            case "Character":
                p1Stats = choiceManager.characterChoices;
                break;

            case "Item":
                playerItemChoices = choiceManager.playerItemChoices;
                break;

            case "Minion":
                //Could add this if wanted.
                break;

            case "Modifier":
                enemyModifierChoices = choiceManager.enemyModifierChoices;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FindChoiceIndex() == choiceIndex)
        {
            //Do nothing
            return;
        }
        Debug.Log("made it down here");
        choiceIndex = FindChoiceIndex();

        Debug.Log("Choice index for tooltip is: " + choiceIndex);
        UpdateTooltip(choiceType);

    }

    private int FindChoiceIndex()
    {
        switch (choiceType)
        {
            case "Character":
                return player1Selector.GetChoiceIndex();
            case "Item":
                return player1Selector.GetChoiceIndex();

            case "Modifier":
                return player2Selector.GetChoiceIndex();
        }
        
        Debug.Log("Was unable to find choice-index, returning 0.");
        return 0;
    }

    void UpdateTooltip(string _choiceType)
    {
        switch (_choiceType)
        {
            case "Character":
                //use description in P1Stats scriptableObject to update tooltip text
                tooltipText = p1Stats[choiceIndex].description;
                break;

            case "Item":
                tooltipText = playerItemChoices[choiceIndex].description;
                break;

            case "Minion":
                //Could add tooltips for minion/monster/enemy types, if necessary/wanted.
                break;

            case "Modifier":
                tooltipText = enemyModifierChoices[choiceIndex].description;
                break;
        }

        //TODO: Set tooltip objects text equal to tooltipText
        textMeshPro.text = tooltipText;
    }
}
