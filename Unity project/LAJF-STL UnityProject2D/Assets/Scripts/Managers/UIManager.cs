using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using STL2.Events;

public class UIManager : MonoBehaviour
{
    //This entire thing would do well to be more state-driven than what it is now.
    //Should keep track of the current UI-state, and handle UI-transitions in general.
    public bool isLoadingScreenOn = false;

    public BoolVariable isGamePaused;

    public IntTypeListener loadProgressUpdateEvent;

    public GameObject loadingScreen;
    public Slider progressBar;

    private bool isWaitingForTimer;
    [SerializeField]
    private float maxTime;
    private float timer; //Used to prevent loadingscreen instantly disappearing/blinking.

    public GameObject pauseCanvasGroup;

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


    //Called by an event on the UIManager object.
    public void UpdateLoadingScreen(int progress)
    {
        if (progress == 0)
        {
            isWaitingForTimer = true;
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
                isWaitingForTimer = false;
                timer = 0;
                CloseLoadingScreen();
                return;
            }
        }
    }

    public void OnPauseButtonClicked(){
        pauseCanvasGroup.SetActive(isGamePaused.myBool);
    }

}