using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p4Vote : Vote
{
    protected override void PlaceVote(int choice)
    {
        this.choice = choice;
        base.PlaceVote(choice);
    }

    protected override void CancelVote()
    {
        base.CancelVote();
    }
}
