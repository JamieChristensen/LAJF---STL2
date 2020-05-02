using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p4Vote : Vote
{

    protected override void Start()
    {
        base.Start();
        Invoke("UpdateVisuals", 0.1f);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void PlaceVote(int choice)
    {
        this.choice = choice;
        base.PlaceVote(choice);

    }

    protected override void CancelVote()
    {
        base.CancelVote();

    }

    protected override void SwitchHover(int addition)
    {
        base.SwitchHover(addition);
    }

    protected override void UpdateVisuals()
    {
        base.UpdateVisuals();
    }
}
