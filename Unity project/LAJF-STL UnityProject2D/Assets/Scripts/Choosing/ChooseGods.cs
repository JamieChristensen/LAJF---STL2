using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using STL2.Events;
using UnityEngine.UI;


public class ChooseGods : MonoBehaviour
{
    public ButtonSounds buttonSounds;
    public TransitionNarrator transitionNarrator;

    public ChoiceCategory runTimeChoices;
    [SerializeField]
    private SettingsScrObj gamesettings;

    public GodInformation[] chooseableGods;

    public TextMeshProUGUI[] choiceTMProText;
    public const int amountOfChoices = 5;

    public IntEvent nextTransition;

    [SerializeField]
    private int[] choices = new int[3]; //player2 choice in 0, player3 choice in 1, player4 choice in 2. Range 1-5. 0 if nothing has been chosen.
    public Image[] backgrounds;
    public Image[] highlights;
    public Image[] overlays;
    private int p2Index, p3Index, p4Index;
    private bool p2LockedIn, p3LockedIn, p4LockedIn;

    [Header("Player2 controls")]
    private bool p2WaitForNextClick = false;
    public KeyCode p2left;
    public KeyCode p2right, p2select;

    public KeyCode p2SelectAlt;
    public string p2HorizontalAxisName;

    [Header("Player3 controls")]
    private bool p3WaitForNextClick = false;
    public KeyCode p3left;
    public KeyCode p3right, p3select;

    public KeyCode p3SelectAlt;
    public string p3HorizontalAxisName;

    [Header("Player4 controls")]
    private bool p4WaitForNextClick = false;
    public KeyCode p4left;
    public KeyCode p4right, p4select;

    public KeyCode p4SelectAlt;
    public string p4HorizontalAxisName;
    bool lockedIn = false;

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
            if (gamesettings.GetAmountOfPlayers() - 2 < i)
            {
                return;
            }
            choiceTMProText[choices[i]].text += "Player " + (i + 2) + "\n";
        }

        if (gamesettings.GetAmountOfPlayers() < 4)
        {
            p4LockedIn = true;
        }
        if (gamesettings.GetAmountOfPlayers() < 3)
        {
            p3LockedIn = true;
        }
    }

    public void Update()
    {
        if (p2LockedIn && p3LockedIn && p4LockedIn && lockedIn == false)
        {
            for (int i = 0; i < choices.Length; i++)
            {
                runTimeChoices.chosenGods[i] = chooseableGods[choices[i]];
            }

            buttonSounds.OnChoiceMade();
            //transitionNarrator.DoNarration();

            Invoke("LoadTransition", 1.5f);
            lockedIn = true;
            return;
        }

        //Messy input-region. Couldn't think of a more concise way to write this without having to make God-Objects to draw from in this script. Not perfect.
        #region Inputs
        int currentPlayerIndex = p2Index; //Index of the player whose character we're checking inputs for.
        if (!p2LockedIn)
        {
            bool p2leftPressed = false;
            bool p2rightPressed = false;
            float p2HoriInput = Input.GetAxis(p2HorizontalAxisName);
            if (p2HoriInput != 0 && p2WaitForNextClick)
            {
                p2leftPressed = p2HoriInput < 0 ? true : false;
                p2rightPressed = !p2leftPressed;
                p2WaitForNextClick = true;
            }
            if (p2HoriInput == 0)
            {
                p2WaitForNextClick = false;
            }

            if (Input.GetKeyDown(p2left) || p2leftPressed)
            {
                int selection = (choices[currentPlayerIndex] + (amountOfChoices - 1)) % amountOfChoices; //Move leftwards in choices.
                ChangeAndDisplaySelection(currentPlayerIndex, selection);
            }
            if (Input.GetKeyDown(p2right) || p2rightPressed)
            {
                int selection = (choices[currentPlayerIndex] + (amountOfChoices + 1)) % amountOfChoices; //Move right in choices.
                ChangeAndDisplaySelection(currentPlayerIndex, selection);
            }
            if (Input.GetKeyDown(p2select) || Input.GetKeyDown(p2SelectAlt))
            {
                p2LockedIn = true;
            }
        }


        currentPlayerIndex = p3Index;
        if (!p3LockedIn)
        {
            bool p3leftPressed = false;
            bool p3rightPressed = false;
            float p3HoriInput = Input.GetAxis(p3HorizontalAxisName);
            if (p3HoriInput != 0 && p3WaitForNextClick)
            {
                p3leftPressed = p3HoriInput < 0 ? true : false;
                p3rightPressed = !p3leftPressed;
                p3WaitForNextClick = true;
            }
            if (p3HoriInput == 0)
            {
                p3WaitForNextClick = false;
            }

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
            bool p4leftPressed = false;
            bool p4rightPressed = false;
            float p4HoriInput = Input.GetAxis(p4HorizontalAxisName);
            if (p4HoriInput != 0 && p4WaitForNextClick)
            {
                p4leftPressed = p4HoriInput < 0 ? true : false;
                p4rightPressed = !p4leftPressed;
                p4WaitForNextClick = true;
            }
            if (p4HoriInput == 0)
            {
                p4WaitForNextClick = false;
            }

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

    public void LoadTransition()
    {
        nextTransition.Raise(4);
    }

    public void LoadNextScene() // Character & Theme Screen
    {
        Debug.Log("Going to next scene!");
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(buildIndex + 1); // go to next scene

    }

    public void ChangeAndDisplaySelection(int godNumber, int newSelection)
    {
        if (godNumber + 2 > gamesettings.GetAmountOfPlayers())
        {
            return;
        }
        choices[godNumber] = newSelection;
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
        for (int i = 0; i < gamesettings.GetAmountOfPlayers() - 1; i++)
        {
            choiceTMProText[choices[i]].text += "Player " + (i + 2) + "\n";
        }
    }

    //When there's only one god
    public void ChangeAndDisplaySelection(int newSelection)
    {
        choices[0] = newSelection;
        choiceTMProText[newSelection].text = "Player2";

        foreach (TextMeshProUGUI choiceText in choiceTMProText)
        {
            choiceText.text = "";
        }
        for (int i = 0; i < gamesettings.GetAmountOfPlayers() - 1; i++)
        {
            choiceTMProText[choices[i]].text += "Player " + (i + 2) + "\n";
        }
    }

}
