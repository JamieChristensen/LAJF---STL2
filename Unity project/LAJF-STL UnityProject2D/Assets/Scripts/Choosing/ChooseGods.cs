﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class ChooseGods : MonoBehaviour
{
    public ChoiceCategory runTimeChoices;

    public GodInformation[] chooseableGods;

    public TextMeshProUGUI[] choiceTMProText;
    public const int amountOfChoices = 5;

    [SerializeField]
    private int[] choices = new int[3]; //player2 choice in 0, player3 choice in 1, player4 choice in 2. Range 1-5. 0 if nothing has been chosen.
    private int p2Index, p3Index, p4Index;
    private bool p2LockedIn, p3LockedIn, p4LockedIn;

    [Header("Player2 controls")]
    public KeyCode p2left;
    public KeyCode p2right, p2select;

    [Header("Player3 controls")]
    public KeyCode p3left;
    public KeyCode p3right, p3select;

    [Header("Player4 controls")]
    public KeyCode p4left;
    public KeyCode p4right, p4select;


    public void Start()
    {
        choices = new int[3];
        p2Index = 0;
        p3Index = 1;
        p4Index = 2;

        foreach (TextMeshProUGUI choiceText in choiceTMProText)
        {
            choiceText.text = "";
        }
        for (int i = 0; i < choices.Length; i++)
        {
            choiceTMProText[choices[i]].text += "Player " + (i + 1) + "\n";
        }
    }

    public void Update()
    {
        if (p2LockedIn && p3LockedIn && p4LockedIn)
        {
            //TODO: For each player, record their choice to Runtimeplayerchoices (ChoiceCategory scriptableObject).
            for (int i = 0; i < choices.Length; i++)
            {
                runTimeChoices.chosenGods[i] = chooseableGods[choices[i]];
            }

            LoadNextScene();
            return;
        }

        //Messy input-region. Couldn't think of a more concise way to write this without having to make God-Objects to draw from in this script. Not perfect.
        #region Inputs
        int currentPlayerIndex = p2Index; //Index of the player whose character we're checking inputs for.
        if (!p2LockedIn)
        {
            if (Input.GetKeyDown(p2left))
            {
                int selection = (choices[currentPlayerIndex] + (amountOfChoices - 1)) % amountOfChoices; //Move leftwards in choices.
                ChangeAndDisplaySelection(currentPlayerIndex, selection);
            }
            if (Input.GetKeyDown(p2right))
            {
                int selection = (choices[currentPlayerIndex] + (amountOfChoices + 1)) % amountOfChoices; //Move right in choices.
                ChangeAndDisplaySelection(currentPlayerIndex, selection);
            }
            if (Input.GetKeyDown(p2select))
            {
                p2LockedIn = true;
            }
        }

        currentPlayerIndex = p3Index;
        if (!p3LockedIn)
        {
            if (Input.GetKeyDown(p3left))
            {
                int selection = (choices[currentPlayerIndex] + (amountOfChoices - 1)) % amountOfChoices;
                ChangeAndDisplaySelection(currentPlayerIndex, selection);
            }
            if (Input.GetKeyDown(p3right))
            {
                int selection = (choices[currentPlayerIndex] + (amountOfChoices + 1)) % amountOfChoices; //Move leftwards in choices.
                ChangeAndDisplaySelection(currentPlayerIndex, selection);
            }
            if (Input.GetKeyDown(p3select))
            {
                p3LockedIn = true;
            }
        }

        currentPlayerIndex = p4Index;
        if (!p4LockedIn)
        {
            if (Input.GetKeyDown(p4left))
            {
                int selection = (choices[currentPlayerIndex] + (amountOfChoices - 1)) % amountOfChoices;
                ChangeAndDisplaySelection(currentPlayerIndex, selection);
            }
            if (Input.GetKeyDown(p4right))
            {
                int selection = (choices[currentPlayerIndex] + (amountOfChoices + 1)) % amountOfChoices;
                ChangeAndDisplaySelection(currentPlayerIndex, selection);
            }
            if (Input.GetKeyDown(p4select))
            {
                p4LockedIn = true;
            }
        }
        #endregion Inputs
    }

    public void LoadNextScene() // Character & Theme Screen
    {
        Debug.Log("Going to next scene!");
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(buildIndex + 1); // go to next scene
    }

    public void ChangeAndDisplaySelection(int godNumber, int newSelection)
    {
        choices[godNumber] = newSelection;
        //Reset text on each element:
        foreach (TextMeshProUGUI choiceText in choiceTMProText)
        {
            choiceText.text = "";
        }
        for (int i = 0; i < choices.Length; i++)
        {
            choiceTMProText[choices[i]].text += "Player " + (i + 1) + "\n";
        }
    }

    //When there's only one god
    public void ChangeAndDisplaySelection(int newSelection)
    {
        choices[0] = newSelection;
        choiceTMProText[newSelection].text = "Player1";
    }
}
