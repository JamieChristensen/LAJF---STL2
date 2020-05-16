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

    public string playerHorizontalAxis;
    public KeyCode selectAlt, returnAlt;
    private bool waitForNextClick;
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
        bool clickedLeft = false;
        bool clickedRight = false;
        float playerHorizontalAxisValue = Input.GetAxis(playerHorizontalAxis);

        

        if (playerHorizontalAxisValue != 0 && !waitForNextClick)
        {
            clickedLeft = playerHorizontalAxisValue < 0 ? true : false;
            clickedRight = !clickedLeft;
            waitForNextClick = true;
        }
        if (playerHorizontalAxisValue == 0)
        {
            waitForNextClick = false;
        }


        if ((Input.GetKeyDown(Left) || clickedLeft) && !lockedIn)
        {
            SwitchHover(-1);
            return;
        }
        if ((Input.GetKeyDown(Right) || clickedRight) && !lockedIn)
        {
            SwitchHover(1);
            return;
        }
        if ((Input.GetKeyDown(Select) || Input.GetKeyDown(selectAlt)) && !lockedIn)
        {
            PlaceVote(choice);
            return;
        }
        if ((Input.GetKeyDown(Return) || Input.GetKeyDown(returnAlt)) && lockedIn)
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
