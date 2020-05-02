using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculatingChoiceFromVotes : MonoBehaviour
{
    public ChoiceCategory runtimeChoices;
    public Image[] godAvatars;

    public p2Vote p2;
    public p3Vote p3;
    public p4Vote p4;

    public SettingsScrObj settingsScirptableObject;

    public int[] playerVotes;
    int amountOfPlayers, amountOfVotes, choice;
    bool p2HasVoted = false, p3HasVoted = false, p4HasVoted = false;


    public ButtonSounds buttonSounds;
    public ChooseBetweenOptionsGiven chooseBetweenOptionsGiven;

    public enum ChoiceType { Theme, Minion, Modifier }
    public ChoiceType choiceType;




    private void Start()
    {
        amountOfPlayers = settingsScirptableObject.GetAmountOfPlayers();
    }

    private void Update()
    {
        CheckForPlayerVotes();
    }

    public void CheckForPlayerVotes()
    {
        if (amountOfPlayers-1 == amountOfVotes)
        {
            CalculateVotes();
            return;
        }
    }

    #region Voting

    public void Player2VotedFor(int choice)
    {
        playerVotes[0] = choice;
        if (playerVotes[0] != 0)
        {
            p2HasVoted = true;
            return;
        }
        p2HasVoted = false;
    }

    public void Player3VotedFor(int choice)
    {
        playerVotes[1] = choice;
        if (playerVotes[1] != 0)
        {
            p3HasVoted = true;
            return;
        }
        p3HasVoted = false;
    }

    public void Player4VotedFor(int choice)
    {
        playerVotes[2] = choice;
        if (playerVotes[2] != 0)
        {
            p4HasVoted = true;
            return;
        }
        p4HasVoted = false;
    }

    #endregion Voting

    public void CalculateVotes()
    {
        switch (amountOfPlayers)
        {
            case 2:
                choice = playerVotes[0];
                ChooseAnOption(choice);
                break;

            case 3:
                CountVotes();
                ChooseAnOption(choice);
                break;

            case 4:
                CountVotes();
                ChooseAnOption(choice);
                break;

            default:
                Debug.LogWarning("The amount of players is not detected in script: CalculatingChoiceFromVotes.cs!");
                break;

        }
    }

   public void ChooseAnOption(int choice)
    {
        string choiceTypeString = choiceType.ToString();

            if (choiceType == ChoiceType.Theme)
            {
            chooseBetweenOptionsGiven.ChooseTheme(choice);
            }

            else if (choiceType == ChoiceType.Minion)
            {
            chooseBetweenOptionsGiven.ChooseMinion(choice);
            }

            else if (choiceType == ChoiceType.Modifier)
            {
            chooseBetweenOptionsGiven.ChooseModifier(choice);
            }

            chooseBetweenOptionsGiven.LockSelectedChoice(choiceTypeString);

            buttonSounds.OnChoiceMade();

            return;
   }

    public void CountVotes()
    {
        int votesFor1 = 0; 
        int votesFor2 = 0;
        int votesFor3 = 0;

        for (int i = 0; i < amountOfPlayers-1;i++)
        {
            switch (playerVotes[i])
            {
                case 1:
                    votesFor1++;
                break;

                case 2:
                    votesFor2++;
                break;

                case 3:
                    votesFor3++;
                break;

                default:
                    playerVotes[i] = RandomizePlayerVote();
                    if (playerVotes[i] == 1)
                        goto case 1;
                    if (playerVotes[i] == 2)
                        goto case 2;
                    if (playerVotes[i] == 3)
                        goto case 3;
                    break;
            }
        }
        if (votesFor1 == votesFor2)
        {
            if (votesFor1 == votesFor3)
            {
                choice = RandomizeChoice();
                return;
            }
            
        }
        else if (votesFor1 == votesFor3)
        {

        }
        else if (votesFor2 == votesFor3)
        {

        }
            

    }

    public int RandomizePlayerVote()
    {
        int randomVote = 0;
        randomVote = Random.Range(1, 4);
        return randomVote;
    }

    public int RandomizeChoice()
    {
        int randomChoice = 0;
        randomChoice = Random.Range(1, 4);
        return randomChoice;
    }

    public int RandomizeChoiceBetweenTwo(int first, int second)
    {
        int randomChoice = 0;
        randomChoice = Random.Range(1, 3);
        if (randomChoice == 1)
        {
            return first;
        }
        else
        {
            return second;
        }

    }




}
