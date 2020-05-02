using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculatingChoiceFromVotes : MonoBehaviour
{

    public p2Vote p2;
    public p3Vote p3;
    public p4Vote p4;

    public SettingsScrObj settingsScirptableObject;

    public int[] playerVotes;
    int amountOfPlayers, amountOfVotes, choice;
    bool p2HasVoted = false, p3HasVoted = false, p4HasVoted = false, openForVotes = true;


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
        if (openForVotes)
        {
            CheckForPlayerVotes();
        }
        
    }

    public void CheckForPlayerVotes()
    {
        if (amountOfPlayers-1 == amountOfVotes)
        {
            CalculateVotes();
            openForVotes = false;
            return;
        }
    }

    public void OutOfTime()
    {
        if (!p2HasVoted)
        {
            playerVotes[0] = 0;
        }
        if (!p3HasVoted)
        {
            playerVotes[1] = 0;
        }
        if (!p4HasVoted)
        {
            playerVotes[2] = 0;
        }
        CalculateVotes();
        openForVotes = false;
        return;
    }

    #region Voting

    public void Player2VotedFor(int choice)
    {
        playerVotes[0] = choice;
        if (playerVotes[0] != 0)
        {
            p2HasVoted = true;
            amountOfVotes++;
            return;
        }
        p2HasVoted = false;
        amountOfVotes--;
    }

    public void Player3VotedFor(int choice)
    {
        playerVotes[1] = choice;
        if (playerVotes[1] != 0)
        {
            p3HasVoted = true;
            amountOfVotes++;
            return;
        }
        p3HasVoted = false;
        amountOfVotes--;
    }

    public void Player4VotedFor(int choice)
    {
        playerVotes[2] = choice;
        if (playerVotes[2] != 0)
        {
            p4HasVoted = true;
            amountOfVotes++;
            return;
        }
        p4HasVoted = false;
        amountOfVotes--;
    }

    #endregion Voting

    public void CalculateVotes()
    {
        p2.enabled = false;
        p3.enabled = false;
        p4.enabled = false;
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

        //Debug.Log(p2HasVoted.ToString() + " " + p3HasVoted.ToString() + " " + p4HasVoted.ToString());
        //Debug.Log("2: " + playerVotes[0] + " - 3: " + playerVotes[1] + " - 4: " + playerVotes[2]);
            return;
   }

    public void CountVotes()
    {
        VotesForNumber votesFor1 = new VotesForNumber();
        VotesForNumber votesFor2 = new VotesForNumber();
        VotesForNumber votesFor3 = new VotesForNumber();
        votesFor1.votes = 0;
        votesFor2.votes = 0;
        votesFor3.votes = 0;
        votesFor1.number = 1;
        votesFor2.number = 2;
        votesFor3.number = 3;

        for (int i = 0; i < amountOfPlayers-1;i++)
        {
            switch (playerVotes[i])
            {
                case 1:
                    votesFor1.votes++;
                break;

                case 2:
                    votesFor2.votes++;
                break;

                case 3:
                    votesFor3.votes++;
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
        List<VotesForNumber> votesForNumber = new List<VotesForNumber>(); 
        votesForNumber.Add(votesFor1);
        votesForNumber.Add(votesFor2);
        votesForNumber.Add(votesFor3);
        votesForNumber.Sort((a, b) => b.votes.CompareTo(a.votes));
        /*
        foreach (VotesForNumber option in votesForNumber)
        {
            //Debug.Log("Votes: " + option.votes + " - Number: " + option.number.ToString());
        }
        */

        if (votesForNumber[0].votes > votesForNumber[1].votes)
        {
            choice = votesForNumber[0].number;
            ChooseAnOption(choice);
            return;
        }
        else if (votesForNumber[0].votes == votesForNumber[1].votes)
        {
            if (votesForNumber[0].votes == votesForNumber[2].votes)
            {
                choice = RandomizeChoice();
                ChooseAnOption(choice);
                return;
            }
            else
            {
                choice = RandomizeChoiceBetweenTwo(votesForNumber[0].number, votesForNumber[1].number);
                ChooseAnOption(choice);
                return;
            }
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

[System.Serializable]
public class VotesForNumber
{
    public int number;
    public int votes;
}
