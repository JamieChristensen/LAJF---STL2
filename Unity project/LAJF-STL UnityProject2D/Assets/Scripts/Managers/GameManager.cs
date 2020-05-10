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

    private MusicManager musicManager;
    public GameObject musicManagerPrefab;

    public NarratorBehaviour narrator;

    public static bool canPlayerMove { get; private set; }
    [SerializeField] private bool[] _canMonsterMove;
    public bool[] canMonsterMove { get { return _canMonsterMove; } }

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
        // runTimeChoises.runTimeLoopCount = 1; // this is the first loop
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
        sceneManager.ChooseChoiceSceneToLoad(indexOfMinionChoiceScene); // get the minion scene ready
    }

    public void GodsPickedMonster()
    {
        sceneManager.ChooseChoiceSceneToLoad(indexOfModifierChoiceScene);
        nextTransition.Raise(10);
    }

    public void GodsHavePickedMonsterAndTrait()
    {

    }

    public void GodsPickedMonsterAndTrait()
    {
        sceneManager.ChooseChoiceSceneToLoad(indexOfGameLoopScene);
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
            if (musicManager != null)
            {
                musicManager.adjustCurrentPlayingVolume(0.4f);
            }
            else
            {
                musicManager = FindObjectOfType<MusicManager>();
                musicManager.adjustCurrentPlayingVolume(0.4f);
            }

        }
        catch
        {
            Debug.Log("there is no music manager");
        }
        string enemyInfo = FindObjectOfType<EnemyBehaviour>().name;
        narrator.Narrate("The Hero has Eliminated " + enemyInfo);
        RequestGameStateChange(GameStates.EncounterEnd); //Done for potential victory-music

    }

    public void OnOpenedChest()
    {
        StartCoroutine(OpenChestCoroutine());
    }

    IEnumerator OpenChestCoroutine()
    {

        try
        {
            if (musicManager != null)
            {
                musicManager.PlayMusic("Peace",1);
            }
            else
            {
                musicManager = FindObjectOfType<MusicManager>();
                musicManager.PlayMusic("Peace",1);
            }
        }
        catch
        {
            Debug.Log("there is no music manager");
        }

        canPlayerMove = false;
        // Time.timeScale = 0f;
        sceneManager.ChooseChoiceSceneToLoad(indexOfItemChoiceScene);
        float extraTime = 0;
        if (GameObject.Find("TextToSpeech").GetComponent<AudioSource>().isPlaying)
        {
            extraTime = 2.5f;
        }
        yield return new WaitForSeconds(1.5f + extraTime);
        nextTransition.Raise(14);
        // StartCoroutine(sceneManager.ALoadEnvironment(indexOfItemChoiceScene)); //6 is the index of item-choice scene.
    }

    public void OnPickedItem()
    {
        StartCoroutine(IncrementRuntimeLoopCount());

        //change environment to next one in line.
        NextEnvironment();
        sceneManager.ChooseChoiceSceneToLoad(indexOfMinionChoiceScene);
        
        // RequestGameStateChange(GameStates.InitializingNextScene);
        Time.timeScale = 1f;
        canPlayerMove = true; //probably shouldn't be here, but just for testing it is for now.
                              // StartCoroutine(sceneManager.AUnloadEnvironment(indexOfItemChoiceScene));
    }

    IEnumerator IncrementRuntimeLoopCount()
    {
        yield return new WaitForSeconds(0.01f);

        if (runTimeChoises.runTimeLoopCount < 6)
        {
            runTimeChoises.runTimeLoopCount++;
        }
        else
        {
            runTimeChoises.runTimeLoopCount = 1;
        }
        nextTransition.Raise(16);
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
        canPlayerMove = false;
        bool dying = true;
        float timer = 0;

        musicManager = FindObjectOfType<MusicManager>();
        musicManager.adjustCurrentPlayingVolume(0.4f);
        while (dying)
        {
            timer += Time.deltaTime;
            if (timer > _timeBetweenPlayerDeathAndEndScreen)
            {
                dying = false;
                StartCoroutine(EndScene(false));
            }
            yield return null;
        }



    }

    public void OnHeroWonRound()
    {
        StartCoroutine(EndScene(true));
    }

    IEnumerator EndScene(bool win)
    {
        
        yield return new WaitForSeconds(1);
        if (win)
        {
            if (musicManager != null)
            {
                musicManager.sources[4].clip = musicManager.ending[0];
                musicManager.PlayMusic("Ending",1);

            }
            else
            {
                musicManager = FindObjectOfType<MusicManager>();
                musicManager.sources[4].clip = musicManager.ending[0];
                musicManager.PlayMusic("Ending",1);

            }

        }
        else
        {
            if (musicManager != null)
            {
                musicManager.sources[4].clip = musicManager.ending[1];
                musicManager.PlayMusic("Ending",1);
            }
            else
            {
                musicManager = FindObjectOfType<MusicManager>();
                musicManager.sources[4].clip = musicManager.ending[1];
                musicManager.PlayMusic("Ending",1);
            }

        }

        yield return new WaitForSeconds(1.5f);
        nextTransition.Raise(18);
    }

}
