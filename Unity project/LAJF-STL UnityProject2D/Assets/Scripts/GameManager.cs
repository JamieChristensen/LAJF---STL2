using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class GameManager : MonoBehaviour
{
    public IntTypeListener playerHPListener;

    public static GameStates gameState = GameStates.none;
    public enum GameStates{
        none, /*Before any gamestate is entered, and perhaps for edge-cases/debugging.*/
        HeroSelect, /*Also when environment-change happens.*/
        SelectingMonster,
        SelectingModifier,
        Encounter, /*When player1 fights the monster, and player2+3+4 emote and throw thunderbolts and stuff at p1.*/
        EncounterEnd, /*When player dies, wins a round or wins vs boss*/
        InitializingNextScene /*When the next encounter is being loaded and/or a transition is being made between screens (not scenes - we're additively loading stuff)*/
    }


    //TODO: 
    //TODO:
    //TODO:



    public void RequestGameStateChange(GameStates requestedState){
        bool isChangeAllowed = false;
        //TODO: Take into account what state is changed to and from.
        switch(requestedState){
            case GameStates.HeroSelect:
            isChangeAllowed = true;
            break; 
            case GameStates.SelectingMonster:
            isChangeAllowed = true;
            break; 
            case GameStates.SelectingModifier:
            isChangeAllowed = true;
            break; 
            case GameStates.Encounter:
            isChangeAllowed = true;
            break; 
            case GameStates.EncounterEnd:
            isChangeAllowed = true;
            break; 
            case GameStates.InitializingNextScene:
            isChangeAllowed = true;
            break; 
        }

        if(isChangeAllowed) {
            ChangeGameState(requestedState);
        }
    }

    private void ChangeGameState(GameStates gameState){
        throw new NotImplementedException();
    }
}
