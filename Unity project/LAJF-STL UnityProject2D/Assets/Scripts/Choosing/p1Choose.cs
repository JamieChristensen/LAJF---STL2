using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class p1Choose : MonoBehaviour
{
    public ButtonSounds buttonSounds;
    public TransitionNarrator transitionNarrator;

    public ChoiceCategory runtimeChoices;
    public string choiceType;
    public ChooseBetweenOptionsGiven chooseBetweenOptionsScript;

    public TextMeshProUGUI[] choiceTMProText;

    public int amountOfChoices = 3;

    public Image[] backgrounds;
    public Image[] highlights;
    public Image[] overlays;

    // The last item pick
    public TextMeshProUGUI victoryChoiceTMProText;
    public Image victoryItemBackground, victoryItemHighlight, victoryItemOverlay;

    [Header("Player1 controls")]
    public KeyCode p1Select;
    public KeyCode p1Left, p1Right;

    bool lockedIn = false;
    bool selected = false;

    [SerializeField]
    private int choice = 0;



    public void Start()
    {
        foreach (TextMeshProUGUI choiceText in choiceTMProText)
        {
            choiceText.text = "";
        }
        choiceTMProText[0].text += "Player 1" + "\n";

        if (choiceType == "Item" && runtimeChoices.runTimeLoopCount == 4)
        {
            amountOfChoices = 1;
        }
        ChangeAndDisplaySelection(0);

    }

    private void Update()
    {
        if (selected == true && lockedIn == false)
        {
            if (choiceType == "Character")
            {
                chooseBetweenOptionsScript.ChooseCharacter(choice + 1);
            }
            else if (choiceType == "Item")
            {
                chooseBetweenOptionsScript.ChooseItem(choice + 1);
            }
            chooseBetweenOptionsScript.LockSelectedChoice(choiceType);

            buttonSounds.OnChoiceMade();
            transitionNarrator.DoNarration();

            lockedIn = true;
            return;
        }

        if (lockedIn == false)
        {
            if (Input.GetKeyDown(p1Left))
            {
                int selection = (choice + (amountOfChoices - 1)) % amountOfChoices; //Move leftwards in choices.
                ChangeAndDisplaySelection(selection);
            }
            if (Input.GetKeyDown(p1Right))
            {
                int selection = (choice + (amountOfChoices + 1)) % amountOfChoices; //Move right in choices.
                ChangeAndDisplaySelection(selection);
            }
            if (Input.GetKeyDown(p1Select))
            {
                selected = true;
            }
        }

    }
    public void ChangeAndDisplaySelection(int newSelection)
    {
        if (amountOfChoices != 1)
        {
            choice = newSelection;
            for (int i = 0; i < backgrounds.Length; i++)
            {
                if (i == newSelection)
                {
                    backgrounds[i].gameObject.SetActive(true);
                    highlights[i].gameObject.SetActive(true);
                    overlays[i].gameObject.SetActive(false);
                }
                else
                {
                    backgrounds[i].gameObject.SetActive(false);
                    highlights[i].gameObject.SetActive(false);
                    overlays[i].gameObject.SetActive(true);
                }
            }

            //Reset text on each element:
            foreach (TextMeshProUGUI choiceText in choiceTMProText)
            {
                choiceText.text = "";
            }
            choiceTMProText[choice].text += "Player 1" + "\n";
        }

        else
        {
            victoryItemBackground.gameObject.SetActive(true);
            victoryItemHighlight.gameObject.SetActive(true);
            victoryItemOverlay.gameObject.SetActive(false);
            victoryChoiceTMProText.text = "";
            victoryChoiceTMProText.text += "Player 1" + "\n";
        }

    }

    public int GetChoiceIndex()
    {
        Debug.Log("Get choice index p1choose");
        return choice;
    }
}
