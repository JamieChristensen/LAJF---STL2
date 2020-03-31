using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int indexOfCharacterChoiceScene, indexOfThemeChoiceScene, indexOfGodChoiceScene, indexOfMinionChoiceScene, indexOfModifierChoiceScene, indexOfItemChoiceScene;
    public IntTypeListener playerHPListener;
    public BoolVariable isGamePaused;

    public BoolVariable isSceneLoading;

    public static bool canPlayerMove { get; private set; }

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
        canPlayerMove = true; //This should be depending on gamestate, but for now it isn't.
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


        canPlayerMove = false;
        Time.timeScale = 0f;
        //  StartCoroutine(sceneManager.ALoadEnvironment(indexOfCharacterChoiceScene));
        sceneManager.StartFade(indexOfCharacterChoiceScene);
    }

    public void PlayerHealthResponse(int playerHP)
    {
        if (playerHP > 0)
        {
            return;
        }

        //Definitely need to implement a timer here.

        //End game by loading proper scene through game-manager - should happen after a delay, as to provide feedback during the delay.
        FindObjectOfType<CustomSceneManager>().LoadCredits(); //
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

    public void EnvironmentAndHeroChoiceFinished()
    {
        //Need to figure out which gamestate to go to - rather than none

        StartCoroutine(sceneManager.AUnloadEnvironment(indexOfThemeChoiceScene));
        RequestGameStateChange(GameStates.None);
        StartCoroutine(sceneManager.ALoadEnvironment(indexOfMinionChoiceScene));

    }

    public void GodsPickedMonster()
    {
        StartCoroutine(sceneManager.AUnloadEnvironment(indexOfMinionChoiceScene));
        StartCoroutine(sceneManager.ALoadEnvironment(indexOfModifierChoiceScene));
    }

    public void GodsPickedMonsterAndTrait()
    {
        Time.timeScale = 1f;
        canPlayerMove = true; //probably shouldn't be here, but just for testing it is for now.
        ChangeGameState(GameStates.Encounter);
        StartCoroutine(sceneManager.AUnloadEnvironment(indexOfModifierChoiceScene));

    }

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

    public void OnMonsterDied()
    {
        RequestGameStateChange(GameStates.EncounterEnd); //Done for potential victory-music   
    }

    public void OnOpenedChest()
    {
        canPlayerMove = false;
        Time.timeScale=0f;
        StartCoroutine(sceneManager.ALoadEnvironment(indexOfItemChoiceScene)); //6 is the index of item-choice scene.
    }

    public void OnPickedItem()
    {
        //change environment to next one in line.
        RequestGameStateChange(GameStates.InitializingNextScene);
        Time.timeScale = 1f;
        canPlayerMove = true; //probably shouldn't be here, but just for testing it is for now.
        StartCoroutine(sceneManager.AUnloadEnvironment(indexOfItemChoiceScene));
    }

    public void OnPickedCharacter()
    {
        StartCoroutine(sceneManager.AUnloadEnvironment(indexOfCharacterChoiceScene));
        StartCoroutine(sceneManager.ALoadEnvironment(indexOfThemeChoiceScene));

    }
}
