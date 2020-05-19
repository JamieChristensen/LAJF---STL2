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

    public string[] tooltipTexts;

    private string choiceType;
    private int choiceIndex;

    //Choice arrays from choicemanaging script:
    private P1Stats[] p1Stats;
    private PlayerItems[] playerItemChoices;
    private EnemyModifier[] enemyModifierChoices;
    private Environment[] environmentThemeChoices;
    private Enemy[] enemyChoices;

    public TextMeshProUGUI[] textMeshPro;

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
                enemyChoices = choiceManager.enemyChoices;
                break;

            case "Modifier":
                enemyModifierChoices = choiceManager.enemyModifierChoices;
                break;

            case "Theme":
                environmentThemeChoices = choiceManager.environmentThemeChoices;
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
        //Debug.Log("made it down here");
        choiceIndex = FindChoiceIndex();

       // Debug.Log("Choice index for tooltip is: " + choiceIndex);
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
                //tooltipText = p1Stats[choiceIndex].description;
                tooltipTexts[0] = p1Stats[0].description;
                tooltipTexts[1] = p1Stats[1].description;
                tooltipTexts[2] = p1Stats[2].description;
                break;

            case "Item":
                if (playerItemChoices[0] != null)
                {
                    tooltipTexts[0] = playerItemChoices[0].description;
                    tooltipTexts[1] = playerItemChoices[1].description;
                    break;
                }
                tooltipTexts[2] = choiceManager.victoryShades.description;
                break;

            case "Minion":
                tooltipTexts[0] = enemyChoices[0].description;
                tooltipTexts[1] = enemyChoices[1].description;
                tooltipTexts[2] = enemyChoices[2].description;
                break;

            case "Modifier":
                tooltipTexts[0] = enemyModifierChoices[0].description;
                tooltipTexts[1] = enemyModifierChoices[1].description;
                tooltipTexts[2] = enemyModifierChoices[2].description;
                break;

            case "Theme":
                tooltipTexts[0] = environmentThemeChoices[0].themeDescription;
                tooltipTexts[1] = environmentThemeChoices[1].themeDescription;
                tooltipTexts[2] = environmentThemeChoices[2].themeDescription;
                break;
        }

        //TODO: Set tooltip objects text equal to tooltipText
        if (tooltipText != "")
        textMeshPro[0].text = tooltipText;
        else
        {
            textMeshPro[0].text = tooltipTexts[0];
            textMeshPro[1].text = tooltipTexts[1];
            textMeshPro[2].text = tooltipTexts[2];
        }
    }
}
