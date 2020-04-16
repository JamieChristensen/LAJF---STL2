using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using STL2.Events;

public class UIManager : MonoBehaviour
{
    //This entire thing would do well to be more state-driven than what it is now.
    //Should keep track of the current UI-state, and handle UI-transitions in general.
    public bool isLoadingScreenOn = false;

    public BoolVariable isGamePaused;

    public IntTypeListener loadProgressUpdateEvent;

    public IntTypeListener playerHPListener;

    public IntEvent nextTransition;

    public GameObject loadingScreen, FadingScreen;
    public Slider progressBar;
    public Slider playerHPSlider;

    private int nextTransitionIndex = 6;

    [SerializeField]
    private float maxTime = 0.2f; //Assign in inspector
    private float timer; //Used to prevent loadingscreen instantly disappearing/blinking.

    public GameObject pauseCanvasGroup;

    private void Start()
    {
        nextTransition.Raise(7);  
    }

    public void Update()
    {

        if (isLoadingScreenOn)
        {
            timer += Time.deltaTime;
            if (timer < maxTime) { return; }
            if (timer >= maxTime && progressBar.value == 100)
            {
                timer = 0;
                CloseLoadingScreen();
                return;
            }
        }
    }

    public void CloseLoadingScreen()
    {
        loadingScreen.SetActive(false);
        isLoadingScreenOn = false;
    }

    public void StartLoadingScreen()
    {
        progressBar.value = 0;
        loadingScreen.SetActive(true);
        isLoadingScreenOn = true;
    }

    public void StartFading()
    {
        loadingScreen.SetActive(true);
        isLoadingScreenOn = true;
    }


    //Called by an event on the UIManager object.
    public void UpdateLoadingScreen(int progress)
    {
        
            if (progress == 0)
            {
                StartLoadingScreen();
                return;
            }

            if (!isLoadingScreenOn)
            {
                return;
            }
            progressBar.value = progress;

            if (progress == 100)
            {
                if (timer >= maxTime)
                {
                    timer = 0;
                    CloseLoadingScreen();
                    return;
                }
            }
        
       
    }

    public void OnPauseButtonClicked(){
        pauseCanvasGroup.SetActive(isGamePaused.myBool);
    }

    public void UpdateHPSlider(int hp){
        playerHPSlider.value = hp;
    }

    public void OnLoadedAdditiveScene()
    {
        ChooseNextTransition();
        Debug.Log("Next Transition Index: " + nextTransitionIndex);
    }


    public void ChooseNextTransition()
    {
        nextTransitionIndex++;
        if (nextTransitionIndex == 18)
        {
            nextTransitionIndex = 7;
        }
    }

    public void OnNextTransition(int transitionIndex)
    {
        nextTransitionIndex = transitionIndex + 1;
    }


    public void NextTransition()
    {
        StartCoroutine(Delay(1.5f));
    }


    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nextTransition.Raise(nextTransitionIndex);
    }
}