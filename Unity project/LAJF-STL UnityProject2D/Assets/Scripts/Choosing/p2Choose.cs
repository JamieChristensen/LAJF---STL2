using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class p2Choose : MonoBehaviour
{
    public ButtonSounds buttonSounds;
    public TransitionNarrator transitionNarrator;

    public string choiceType;

    public ChooseBetweenOptionsGiven chooseBetweenOptionsScript;

    public TextMeshProUGUI[] choiceTMProText;
    public const int amountOfChoices = 3;

    public Image[] backgrounds;
    public Image[] highlights;
    public Image[] overlays;

    [Header("Player2 controls")]
    public KeyCode p2Select;
    public KeyCode p2Left, p2Right;

    bool lockedIn = false;
    bool selected = false;
    private int choice = 0;

    public void Start()
    {
        foreach (TextMeshProUGUI choiceText in choiceTMProText)
        {
            choiceText.text = "";
        }
        choiceTMProText[0].text += "Player 2" + "\n";
        ChangeAndDisplaySelection(0);
    }
    private void Update()
    {
        if (selected == true && lockedIn == false)
        {
            if (choiceType == "Theme")
            {
                chooseBetweenOptionsScript.ChooseTheme(choice + 1);
            }
            else if (choiceType == "Minion")
            {
                chooseBetweenOptionsScript.ChooseMinion(choice + 1);
            }
            else if (choiceType == "Modifier")
            {
                chooseBetweenOptionsScript.ChooseModifier(choice + 1);
            }
            chooseBetweenOptionsScript.LockSelectedChoice(choiceType);

            buttonSounds.OnChoiceMade();
            transitionNarrator.DoNarration();

            lockedIn = true;
            return;
        }

        if (lockedIn == false)
        {
            if (Input.GetKeyDown(p2Left))
            {
                int selection = (choice + (amountOfChoices - 1)) % amountOfChoices; //Move leftwards in choices.
                ChangeAndDisplaySelection(selection);
            }
            if (Input.GetKeyDown(p2Right))
            {
                int selection = (choice + (amountOfChoices + 1)) % amountOfChoices; //Move right in choices.
                ChangeAndDisplaySelection(selection);
            }
            if (Input.GetKeyDown(p2Select))
            {
                selected = true;
            }
        }

    }
    public void ChangeAndDisplaySelection(int newSelection)
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

            //Reset text on each element:
            foreach (TextMeshProUGUI choiceText in choiceTMProText)
            {
                choiceText.text = "";
            }

            choiceTMProText[choice].text += "Player 2" + "\n";
        }
    }
}