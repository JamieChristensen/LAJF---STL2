using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class Vote : MonoBehaviour
{
    public IntEvent playerVotesForChoiceIndex;
    public int choice = 0;

    [Header("Player2 controls")]
    public KeyCode Select;
    public KeyCode Left, Right, Return;

    protected virtual void Update()
    {
        
    }

    protected virtual void PlaceVote(int choice)
    {
        playerVotesForChoiceIndex.Raise(choice);
    }

    protected virtual void CancelVote()
    {
        choice = 0;
        playerVotesForChoiceIndex.Raise(choice);
    }
}
