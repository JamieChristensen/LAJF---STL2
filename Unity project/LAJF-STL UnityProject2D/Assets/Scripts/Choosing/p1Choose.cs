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

    public KeyCode p1SelectAlt;
    [Tooltip("Must match an Input-Axis")]
    public string p1SelectAltAxisName;

    bool lockedIn = false;
    bool selected = false;

    bool pressedDownHorizontalAxis;
    float horizontalAxisValue;

    [SerializeField]
    private int choice = 0;



    public void Start()
    {
        foreach (TextMeshProUGUI choiceText in choiceTMProText)
        {
            choiceText.text = "";
        }
        choiceTMProText[0].text += "P1" + "\n";

        if (choiceType == "Item" && runtimeChoices.runTimeLoopCount == 4)
        {
            amountOfChoices = 1;
        }
        ChangeAndDisplaySelection(0);

    }

    private void Update()
    {
        #region Alternate/ControllerInput
        //Debug.Log(Input.GetAxis(p1SelectAltAxisName) + " P1 Horizontal axis input");
        if (Input.GetAxis(p1SelectAltAxisName) != 0 && !pressedDownHorizontalAxis)
        {
            pressedDownHorizontalAxis = true;
            horizontalAxisValue = Input.GetAxis(p1SelectAltAxisName);
        }
        if (Input.GetAxis(p1SelectAltAxisName) == 0)
        {
            pressedDownHorizontalAxis = false;
        }
        #endregion Alternate/ControllerInput


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
            //transitionNarrator.DoNarration();

            lockedIn = true;
            return;
        }

        if (lockedIn == false)
        {

            if (Input.GetKeyDown(p1Left) || horizontalAxisValue < 0)
            {
                int selection = (choice + (amountOfChoices - 1)) % amountOfChoices; //Move leftwards in choices.
                ChangeAndDisplaySelection(selection);
                horizontalAxisValue = 0;
            }
            if (Input.GetKeyDown(p1Right) || horizontalAxisValue > 0)
            {
                int selection = (choice + (amountOfChoices + 1)) % amountOfChoices; //Move right in choices.
                ChangeAndDisplaySelection(selection);
                horizontalAxisValue = 0;
            }
            if (Input.GetKeyDown(p1Select) || Input.GetKeyDown(p1SelectAlt))
            {
                selected = true;
            }
        }

    }

    public void OverRuleRandomSelect(string choiceType)
    {
        selected = true;
        this.choiceType = choiceType;
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
            choiceTMProText[choice].text += "P1" + "\n";
        }

        else
        {
            victoryItemBackground.gameObject.SetActive(true);
            victoryItemHighlight.gameObject.SetActive(true);
            victoryItemOverlay.gameObject.SetActive(false);
            victoryChoiceTMProText.text = "";
            victoryChoiceTMProText.text += "P1" + "\n";
        }

    }

    public int GetChoiceIndex()
    {
        Debug.Log("Get choice index p1choose");
        return choice;
    }
}
