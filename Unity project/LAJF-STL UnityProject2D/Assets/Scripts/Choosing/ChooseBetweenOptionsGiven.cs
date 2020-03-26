using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;
using UnityEngine.UI;
using TMPro;

public class ChooseBetweenOptionsGiven : MonoBehaviour
{
    #region INSPECTOR
    public VoidEvent godshaveChosenTheme;
    public VoidEvent godshaveChosenOpponent;
    public VoidEvent heroHasChosenItem;

    public ChoiceCategory character;
    public ChoiceCategory theme;
    public ChoiceCategory minion;
    public ChoiceCategory modifier;
    public PlayerItems item;

    public ChoiceCategory type;
    public ChoiceCategory runtimeChoices;

    public GameObject finalChoice; // the gameObject that has been chosen

    private int choice = 4; // this is 4 by default as there never will be 4 choices to choose from. The forth choice is blindpick.

    [Header("Item choice variables")]
    private PlayerItems[] playerItemChoices = new PlayerItems[2];
    public PlayerItems[] playerItemPool;
    public TextMeshProUGUI[] choiceNameText;
    public Image[] itemImageTargets;

    #endregion // INSPECTOR

    private void Awake()
    {
        choice = 4; // the choice is set back to the default value

        #region InitializeItemSelection
        int random = Random.Range(0, playerItemPool.Length);
        int random2 = random;
        while (random2 == random)
        {
            Debug.Log("random2 was the same as random, rerolling");
            random2 = Random.Range(0, playerItemPool.Length);
        }
        playerItemChoices[0] = playerItemPool[random];
        playerItemChoices[1] = playerItemPool[random2];
        choiceNameText[0].text = playerItemChoices[0].name;
        choiceNameText[1].text = playerItemChoices[1].name;
        itemImageTargets[0].sprite = playerItemChoices[0].itemSprite;
        itemImageTargets[1].sprite = playerItemChoices[1].itemSprite;

        #endregion InitializeItemSelection

    }


    #region choosing

    public void ChooseCharacter(int characterIndex)
    {
        choice = characterIndex;
    }

    public void ChooseTheme(int themeIndex)
    {
        choice = themeIndex;
    }

    public void ChooseMinion(int minionIndex)
    {
        choice = minionIndex;
    }

    public void ChooseModifier(int modifierIndex)
    {
        choice = modifierIndex;
    }

    public void ChooseItem(int itemIndex)
    {
        choice = itemIndex;
    }
    #endregion // choosing


    #region LockingChoice
    public void LockSelectedChoice(string choiceType)
    {

        ConvertChoiceToGameObject(choice, choiceType);

        //  RaiseEvent(choiceType);
    }

    void ConvertChoiceToGameObject(int choice, string choiceType)
    {
        int indexMaximum; // how many things can you choose from

        switch (choiceType)
        {
            case "Item":
                //type = item;
                indexMaximum = 2;
                break;

            case "Character":
                type = character;
                goto default;

            case "Minion":
                type = minion;
                goto default;

            case "Modifier":
                type = modifier;
                goto default;

            case "Theme":
                type = theme;
                goto default;

            default: // Character, Minion, Modifier, Theme

                indexMaximum = 3;
                break;
        }

        switch (choice)
        {
            case 1:
                finalChoice = type.Options[choice - 1];
                break;

            case 2:
                finalChoice = type.Options[choice - 1];
                break;

            case 3:

                if (indexMaximum == 3)
                {
                    finalChoice = type.Options[choice - 1];
                }
                else
                {
                    goto default;
                }

                break;

            default:

                choice = Random.Range(1, indexMaximum + 1);
                finalChoice = type.Options[choice - 1];
                this.choice = choice;
                break;
        }

        RaiseEvent(choiceType);
    }

    #endregion // LockingChoice



    #region RaisingEvents

    void RaiseEvent(string choiceType)
    {

        switch (choiceType)
        {
            case "Item":
                HeroHasChosenItem();
                break;

            case "Character":
                HeroHasChosenCharacter();
                break;

            case "Minion":
                GodsHaveChosenMinion();
                break;

            case "Modifier":
                GodsHaveChosenOpponent();
                break;

            case "Theme":
                GodsHaveChosenTheme();
                break;

            default:

                break;
        }

    }


    void HeroHasChosenCharacter()
    {
        runtimeChoices.character = finalChoice;
        Debug.Log("Hero has chosen a character! It is: " + finalChoice.name);
        SwitchToThemeSelection(); // switching from character select to theme select
    }

    void GodsHaveChosenTheme()
    {
        runtimeChoices.theme = finalChoice;
        Debug.Log("Gods have chosen a theme! It is: " + finalChoice.name);
        godshaveChosenTheme.Raise(); // Raising event for theme chosen
    }

    void GodsHaveChosenMinion()
    {
        switch (runtimeChoices.runTimeLoopCount)
        {
            case 1:
                runtimeChoices.firstOpponent.minion = finalChoice;
                break;
            case 2:
                runtimeChoices.secondOpponent.minion = finalChoice;
                break;
            case 3:
                runtimeChoices.thirdOpponent.minion = finalChoice;
                break;
            case 4:
                runtimeChoices.fourthOpponent.minion = finalChoice;
                break;
        }
        Debug.Log("Gods have chosen the " + runtimeChoices.runTimeLoopCount + ". minion! It is: " + finalChoice.name);
        SwitchToModifierSelection(); // switching from minion select to modifier select
    }

    void GodsHaveChosenOpponent()
    {
        switch (runtimeChoices.runTimeLoopCount)
        {
            case 1:
                runtimeChoices.firstOpponent.modifier = finalChoice;
                break;
            case 2:
                runtimeChoices.secondOpponent.modifier = finalChoice;
                break;
            case 3:
                runtimeChoices.thirdOpponent.modifier = finalChoice;
                break;
            case 4:
                runtimeChoices.fourthOpponent.modifier = finalChoice;
                runtimeChoices.runTimeLoopCount = 1;
                break;
        }
        Debug.Log("Gods have chosen the " + runtimeChoices.runTimeLoopCount + ". modifier! It is: " + finalChoice.name);
        godshaveChosenOpponent.Raise(); // Raising event for opponent chosen
    }

    void HeroHasChosenItem()
    {
        switch (runtimeChoices.runTimeLoopCount)
        {
            case 1:
                runtimeChoices.firstItem = finalChoice;
                break;
            case 2:
                runtimeChoices.secondItem = finalChoice;
                break;
            case 3:
                runtimeChoices.thirdItem = finalChoice;
                break;
        }
        Debug.Log("Hero has chosen the" + runtimeChoices.runTimeLoopCount + ". item! It is: " + finalChoice.name);

        // runtimeChoices.runTimeLoopCount++;  
        runtimeChoices.playerItems.Add(playerItemChoices[choice-1]);
        heroHasChosenItem.Raise(); // Raising event for item chosen
        Destroy(gameObject);
    }


    #endregion // RaisingEvents


    void SwitchToThemeSelection() // from character selection
    {
        Debug.Log("switching to theme select!");

    }

    void SwitchToModifierSelection() // from minion selection
    {
        Debug.Log("switching to modifier select!");


    }



    public void ResetAllChoices()
    {
        /* Local */
        type = null;
        finalChoice = null;
        choice = 4;

        /* in scriptableObject */
        runtimeChoices.runTimeLoopCount = 1;
        runtimeChoices.character = null;
        runtimeChoices.theme = null;
        runtimeChoices.firstOpponent.minion = null;
        runtimeChoices.secondOpponent.minion = null;
        runtimeChoices.thirdOpponent.minion = null;
        runtimeChoices.fourthOpponent.minion = null;
        runtimeChoices.firstOpponent.modifier = null;
        runtimeChoices.secondOpponent.modifier = null;
        runtimeChoices.thirdOpponent.modifier = null;
        runtimeChoices.fourthOpponent.modifier = null;
        runtimeChoices.firstItem = null;
        runtimeChoices.secondItem = null;
        runtimeChoices.thirdItem = null;
    }

}
