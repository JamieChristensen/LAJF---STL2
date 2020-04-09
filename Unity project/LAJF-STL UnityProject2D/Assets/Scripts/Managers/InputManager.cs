using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    //TODO: Change these objects to corresponding classes as they are implemented.
    [SerializeField]
    private P1Controller player1;


    [SerializeField]
    private List<GodController> gods; //Indexed - order matters for inputs.

    [SerializeField]
    private SettingsScrObj gameSettings;

    private int amountOfPlayers, amountOfGods;
    //Should just be an array of gods, loop through the input axes using index
    //when for-looping through them.
    //Initialize "gods" when the amount of players has been selected.

    [Header("Player 1 controls")]
    public KeyCode player1JumpKey;
    public KeyCode player1AttackKey;
    private float player1Hori;
    public KeyCode player1Left, player1Right;
    private bool player1Jump;
    private bool player1JumpHold;
    private bool player1Attack;

    private float doubleTapMaxTime = 0.5f;
    private float tapTimerLeft, tapTimerRight;
    private bool tapTimingLeft, tapTimingRight;
    private float tapCooldownMaxTime = 0.5f;
    private float tapCooldownTimer;
    private bool tapCoolingDown;
    private bool justTapped;

    [Header("Player 2, 3 and 4 controls")]
    public KeyCode[] godLeftInput;
    public KeyCode[] godRightInput;
    public KeyCode[] godSelectInput;
    public KeyCode[] godShootInput;

    //These 3 "current"-inputs are used when iterating through the gods (in GetInputs()) to send the respective gods their respective inputs.
    //I'm doing it this way, so we can manipulate inputs (such as lerping between inputs or have inputs cancel 
    //eachother out before it gets to the god-controller and such). - Jamie
    private float currentGodHorizontalInput;
    private bool currentGodSelectInput;
    private bool currentGodShootInput;

    //TODO: Figure out which inputs to use
    //Probably need to implement border-style selection of portraits for each god and just have left-right inputs to decide which one to go to
    //based on current one.

    //Player1 will have WASD/arrowbuttons for control, and spacebar + some button, for shooting (and a proper control scheme for controllers)
    //Players2,3,4 will need some alternate controls on keyboard too - but otherwise controller usage too.

    private void Start()
    {
        //Alternatively, assign gods in inspector and figure out a way to disable superfluous gods.
        amountOfPlayers = gameSettings.GetAmountOfPlayers();
        amountOfGods = amountOfPlayers - 1;
    }

    private void Update()
    {
        GetInputs();
        SendHeroInputs();

        tapTimerLeft = tapTimingLeft ? tapTimerLeft + Time.deltaTime : 0;
        tapTimerRight = tapTimingRight ? tapTimerRight + Time.deltaTime : 0;
        tapCooldownTimer = tapCoolingDown ? tapCooldownTimer + Time.deltaTime : 0;

        if (tapTimerLeft > doubleTapMaxTime)
        {
            tapTimerLeft = 0;
            tapTimingLeft = false;
        }
        if (tapTimerRight > doubleTapMaxTime)
        {
            tapTimerRight = 0;
            tapTimingRight = false;
        }
        if (tapCooldownTimer > tapCooldownMaxTime)
        {
            tapCoolingDown = false;
            tapCooldownTimer = 0;
        }
    }

    private void GetInputs()
    {
        if (player1 != null)
        {

            float left = Input.GetKey(player1Left) ? 1 : 0;
            float right = Input.GetKey(player1Right) ? 1 : 0;
            player1Hori = right - left;

            player1Jump = Input.GetKeyDown(player1JumpKey);
            player1JumpHold = Input.GetKey(player1JumpKey);
            player1Attack = Input.GetKeyDown(player1AttackKey);


            if (Input.GetKeyDown(player1Left))
            {
                if (tapTimingRight)
                {
                    tapTimingRight = false;
                }
                if (tapTimerLeft < doubleTapMaxTime && tapTimingLeft)
                {
                    justTapped = true;
                    return;
                }
                tapTimingLeft = true;

            }
            if (Input.GetKeyDown(player1Right))
            {
                if (tapTimingLeft)
                {
                    tapTimingLeft = false;
                }
                if (tapTimerRight < doubleTapMaxTime && tapTimingRight)
                {
                    justTapped = true;
                    return;
                }
                tapTimingRight = true;

            }
        }






        for (int i = 0; i < amountOfGods; i++)
        {
            if (gods[i] == null) { continue; }
            float left = Input.GetKeyDown(godLeftInput[i]) ? -1 : 0;
            float right = Input.GetKeyDown(godRightInput[i]) ? 1 : 0;
            currentGodHorizontalInput = right + left;

            currentGodSelectInput = Input.GetKeyDown(godSelectInput[i]);
            currentGodShootInput = Input.GetKeyDown(godShootInput[i]);

            SendGodInputs(gods[i]);
        }

    }

    private void SendHeroInputs()
    {
        if (player1 != null && GameManager.canPlayerMove)
        {
            if (player1Attack)
            {
                player1.ReceiveInput(P1Controller.Player1Input.Attack, 0);  //Float doesn't matter for attack
            }

            if (player1Jump)
            {
                player1.ReceiveInput(P1Controller.Player1Input.Jump, 0); //Float doesn't matter for jump
            }

            if (player1JumpHold)
            {
                player1.ReceiveInput(P1Controller.Player1Input.JumpHold, 1);
            }
            else
            {
                player1.ReceiveInput(P1Controller.Player1Input.JumpHold, 0);
            }

            player1.ReceiveInput(P1Controller.Player1Input.Horizontal, player1Hori);

            if (justTapped && tapTimingLeft)
            {
                justTapped = false;
                tapTimingLeft = false;
                tapTimerLeft = 0;
                tapCoolingDown = true;
                player1.ReceiveInput(P1Controller.Player1Input.DoubleTapLeft, 0);
            }

            if (justTapped && tapTimingRight)
            {
                justTapped = false;
                tapTimingRight = false;
                tapTimerRight = 0;
                tapCoolingDown = true;

                player1.ReceiveInput(P1Controller.Player1Input.DoubleTapRight, 0);
            }


        }

        //Implement gods
    }

    private void SendGodInputs(GodController god)
    {
        //Implement input-actions in godcontroller.cs, then call that here. 

        if (currentGodSelectInput)
        {
            god.Emote();
        }
    }
}
