using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int indexOfGameLoopScene, indexOfMinionChoiceScene, indexOfModifierChoiceScene, indexOfItemChoiceScene;
    public IntTypeListener playerHPListener;
    public BoolVariable isGamePaused;

    public IntEvent nextTransition;

    public BoolVariable isSceneLoading;

    public ChoiceCategory runTimeChoises;

    public MusicManager musicManager;

    public NarratorBehaviour narrator;

    public static bool canPlayerMove { get; private set; }
    [SerializeField] private bool[] _canMonsterMove; 
    public bool[] canMonsterMove { get { return _canMonsterMove; }}

    public VoidTypeListener environmentAndHeroChoiceFinished;  //When the gods finished picking the environment.
    public VoidTypeListener godsPickedMonsterAndTrait;      //Should be raised when gods finish selecting both monster and traits.

    //KVP of gamestates and their given possible states to transition to - used to prevent unwanted transitions.
    private KeyValuePair<GameStates, GameStates[]>[] possibleGameStateTransitions = {
        new KeyValuePair<GameStates, GameStates[]>(GameStates.HeroSelect, new GameStates[] {GameStates.SelectingMonster}),

        new KeyValuePair<GameStates, GameStates[]>(GameStates.SelectingMonster, new GameStates[] {GameStates.SelectingModifier}),

        new KeyValuePair<GameStates, GameStates[]>(GameStates.SelectingModifier, new GameStates[] {GameStates.Encounter}),

        new KeyValuePair<GameStates, GameStates[]>(GameStates.Encounter, new GameStates[] {GameStates.EncounterEnd}),

        new KeyValuePair<GameStates, GameStates[]>(GameStates.EncounterEnd, new GameStates[] {GameStates.InitializingNextScene}),

        new KeyValuePair<GameStates, GameStates[]>(GameStates.InitializingNextScene, new GameStates[] {GameStates.SelectingMonster})
    };

    [SerializeField] private float _timeBetweenPlayerDeathAndEndScreen = 5;


    [SerializeField]
    private GameStates initialGamestate = GameStates.Encounter;

    public static GameStates gameState = GameStates.None;
    public enum GameStates
    {
        None, /*Before any gamestate is entered, and perhaps for edge-cases/debugging.*/
        HeroSelect, /*Also when environment-change happens.*/
        SelectingMonster,
        SelectingModifier,
        Encounter, /*When player1 fights the monster, and player2+3+4 emote and throw thunderbolts and stuff at p1.*/
        EncounterEnd, /*When player dies, wins a round or wins vs boss*/
        InitializingNextScene /*When the next encounter is being loaded and/or a transition is being made between screens (not scenes - we're additively loading stuff)*/
    }

    private CustomSceneManager sceneManager;

    //TODO: Behaviour when player234 finish selection.

    public void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        /*
        for (int i = 0; i < canMonsterMove.Length; i++)
        {
            canMonsterMove[i] = true; //This should be depending on gamestate, but for now it isn't.
        }
        */
        _canMonsterMove[0] = false;

       // Debug.Log("Index to Load: " + runTimeChoises.chosenEnvironments[0].environmentIndex.ToString());
        


         canPlayerMove = true;

        gameState = initialGamestate;
        // GameObject.DontDestroyOnLoad(this);

        if (sceneManager == null)
        {
            sceneManager = FindObjectOfType<CustomSceneManager>();
        }


        //Ensure all cross-scene reference scriptableObjects are initialized properly:
        if (isGamePaused.myBool)
        {
            isGamePaused.setBool(false);
        }
        runTimeChoises.runTimeLoopCount = 1; // this is the first loop
        NextEnvironment();
    }

    public void NextEnvironment()
    {
        sceneManager.RequestEnvironmentChange(runTimeChoises.chosenEnvironments[runTimeChoises.runTimeLoopCount - 1].environmentIndex); // Changing environment to the requested environment in the array (depending on runtime loop count)
    }

    public void PlayerHealthResponse(int playerHP)
    {
        if (playerHP > 0)
        {
            return;
        }

        StartCoroutine(PlayerDeath()); // if Hero dies

    }

    public void RequestGameStateChange(GameStates requestedState)
    {
        bool isChangeAllowed = isTransitionAllowed(gameState, requestedState);
        //Note: If state-changes have to happen in particular ways, a switch-case should be implemented to take these into account.
        if (!isChangeAllowed)
        {
            Debug.Log("Request to change state from: " + gameState + " to " + requestedState + " denied");
            return;
        }
        ChangeGameState(requestedState);
    }

    private bool isTransitionAllowed(GameStates currentState, GameStates desiredState)
    {
        foreach (KeyValuePair<GameStates, GameStates[]> keyValuePair in possibleGameStateTransitions)
        {
            if (keyValuePair.Key == currentState)
            {
                return keyValuePair.Value.Contains(desiredState);
            }
        }
        return false;
    }

    private void ChangeGameState(GameStates _desiredState)
    {
        if (isTransitionAllowed(gameState, _desiredState))
        {
            gameState = _desiredState;
            //Call some function here that figures out what things should change in the game depending on the new gamestate (such as whether player can move or not)
            return;
        }
        Debug.Log("Gamestate transition denied");
    }

    #region EventsListenedTo
    public void HeroHasBeenCaptured()
    {
        //Need to figure out which gamestate to go to - rather than none

        RequestGameStateChange(GameStates.None);
        sceneManager.ChooseSceneToLoad(indexOfMinionChoiceScene); // get the minion scene ready
    }

    public void GodsPickedMonster()
    {
        sceneManager.ChooseSceneToLoad(indexOfModifierChoiceScene);
        nextTransition.Raise(10);
    }

    public void GodsHavePickedMonsterAndTrait()
    {
        
    }

    public void GodsPickedMonsterAndTrait()
    {
        sceneManager.ChooseSceneToLoad(indexOfGameLoopScene);
        nextTransition.Raise(12);
        Time.timeScale = 1f;
        canPlayerMove = true; //probably shouldn't be here, but just for testing it is for now.
        Invoke("SpawnTheMonster", 8);
    }

    public void SpawnTheMonster()
    {
        _canMonsterMove[0] = true; //probably shouldn't be here, but just for testing it is for now.
        ChangeGameState(GameStates.Encounter);
    }


    public void OnMonsterDied()
    {
        try
        {
            musicManager.adjustCurrentPlayingVolume(0.4f);
        }
        catch
        {
            Debug.Log("there is no music manager");
        }

        narrator.Narrate("The Hero has Eliminated " + FindObjectOfType<EnemyBehaviour>().nameUI.text);
        RequestGameStateChange(GameStates.EncounterEnd); //Done for potential victory-music
        
    }

    public void OnOpenedChest()
    {
        try
        {
            musicManager.PlayMusic("Peace");
        }
        catch
        {
            Debug.Log("there is no music manager");
        }
        
        
        canPlayerMove = false;
       // Time.timeScale = 0f;
        sceneManager.ChooseSceneToLoad(indexOfItemChoiceScene);
        nextTransition.Raise(14);
        // StartCoroutine(sceneManager.ALoadEnvironment(indexOfItemChoiceScene)); //6 is the index of item-choice scene.
    }

    public void OnPickedItem()
    {
        if (runTimeChoises.runTimeLoopCount<5)
        {
            runTimeChoises.runTimeLoopCount++;
        }
        else
        {
            runTimeChoises.runTimeLoopCount = 1;
        }

        NextEnvironment();
        //change environment to next one in line.
        sceneManager.ChooseSceneToLoad(indexOfGameLoopScene);
        nextTransition.Raise(16);
        RequestGameStateChange(GameStates.InitializingNextScene);
        Time.timeScale = 1f;
        canPlayerMove = true; //probably shouldn't be here, but just for testing it is for now.
       // StartCoroutine(sceneManager.AUnloadEnvironment(indexOfItemChoiceScene));
    }


    #endregion

    //This is a very brutish way of pausing, but it should work. 
    public void OnPauseButtonClicked()
    {
        isGamePaused.setBool(!isGamePaused.myBool);

        if (isGamePaused.myBool)
        {
            Time.timeScale = 0;
            return;
        }

        if (!isGamePaused.myBool)
        {
            Time.timeScale = 1;
            return;
        }
    }


    IEnumerator PlayerDeath()
    {
        for (int i = 0; i < canMonsterMove.Length; i++)
        {
            canMonsterMove[i] = false;
        }

        try
        {
            musicManager.PlayMusic("Ending");
        }
        catch
        {
            Debug.Log("there is no music manager");
        }

        canPlayerMove = false;
        bool dying = true;
        float timer = 0;
        while (dying)
        {
            timer += Time.deltaTime;
            if (timer > _timeBetweenPlayerDeathAndEndScreen)
            {

                FindObjectOfType<CustomSceneManager>().LoadCredits(); //End game by loading proper scene through game-manager - should happen after a delay, as to provide feedback during the delay.
            }
            yield return null;
        }

    }

}
