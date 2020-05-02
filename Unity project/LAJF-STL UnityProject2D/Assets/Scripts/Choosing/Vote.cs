using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;
using UnityEngine.UI;

public class Vote : MonoBehaviour
{
    public int playerNumber;
    public ChoiceCategory runtimeChoices;
    public SettingsScrObj settingsScirptableObject;
    public IntEvent playerVotesForChoiceIndex;
    public int choice = 1;
    public bool lockedIn = false;
    public Image godAvatar, highLight;
    public GameObject godAvatarGameObject;
    public Transform[] voteLocationTransforms;


    [Header("Player2 controls")]
    public KeyCode Select;
    public KeyCode Left, Right, Return;


    protected virtual void Start()
    {
        godAvatar.sprite = runtimeChoices.chosenGods[playerNumber - 2].topBarIcon;
        godAvatarGameObject.transform.position = voteLocationTransforms[1].position;
        if (settingsScirptableObject.GetAmountOfPlayers() < playerNumber)
        {
            godAvatar.enabled = false;
        }
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(Left) && !lockedIn)
        {
            SwitchHover(-1);
            return;
        }
        if (Input.GetKeyDown(Right) && !lockedIn)
        {
            SwitchHover(1);
            return;
        }
        if (Input.GetKeyDown(Select) && !lockedIn)
        {
            PlaceVote(choice);
            return;
        }
        if (Input.GetKeyDown(Return) && lockedIn)
        {
            CancelVote();
            return;
        }
    }

    protected virtual void PlaceVote(int choice)
    {
        lockedIn = true;
        playerVotesForChoiceIndex.Raise(choice);
        UpdateVisuals();
    }

    protected virtual void CancelVote()
    {
        lockedIn = false;
        choice = 0;
        playerVotesForChoiceIndex.Raise(choice);
        choice = 1;
        UpdateVisuals();
    }

    protected virtual void SwitchHover(int addition)
    {
        choice += addition;
        if (choice > 3)
        {
            choice = 1;
        }
        else if (choice < 1)
        {
            choice = 3;
        }
        UpdateVisuals();
    }

    protected virtual void UpdateVisuals()
    {
        highLight.enabled = lockedIn;
        godAvatarGameObject.transform.position = voteLocationTransforms[choice - 1].position;
        
    }
}
