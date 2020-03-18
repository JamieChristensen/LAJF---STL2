using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public IntTypeListener playerHPListener;
    public BoolVariable isGamePaused;

    public BoolVariable isSceneLoading;
    public IntVariable gameLoadProgress;

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


    //These comments are my todo-list for this script - Jamie
    //TODO: Behaviour when player1 dies.
    //TODO: Behaviour when player234 finish selection.

    public void Start(){
        GameObject.DontDestroyOnLoad(this);

        //Ensure all cross-scene reference scriptableObjects are initialized properly:
        if (isGamePaused.myBool) {
            isGamePaused.setBool(false);
        }
    }

    public void PlayerHealthResponse(int playerHP){
        if (playerHP > 0){
            return;
        }

        //End game by loading proper scene through game-manager - should happen after a delay, as to provide feedback during the delay.
        throw new NotImplementedException();
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
        if (isTransitionAllowed(gameState, _desiredState)){
            gameState = _desiredState;
            return;
        }
        Debug.Log("Gamestate transition denied");
    }

    public void EnvironmentAndHeroChoiceFinished(){
        //Need to figure out which gamestate to go to - rather than none
        RequestGameStateChange(GameStates.None);
    }

    public void GodsPickedMonsterAndTrait(){
        ChangeGameState(GameStates.Encounter);
    }

    //This is a very brutish way of pausing, but it should work. 
    public void OnPauseButtonClicked(){
        isGamePaused.setBool(!isGamePaused.myBool);

        if (isGamePaused.myBool){
            Time.timeScale = 0;
            return;
        }

        if (!isGamePaused.myBool){
            Time.timeScale = 1;
            return;
        }
    }

}
