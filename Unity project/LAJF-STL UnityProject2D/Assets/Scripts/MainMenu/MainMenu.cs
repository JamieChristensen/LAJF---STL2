using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGameButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button exitGameButton;
    
    public GameObject settingsMenu;
    public GameObject creditsMenu;

    public TransitionScreen introTransition;
    public RuntimeChoiceManager runtimeChoiceManager;


    public void StartFading()
    {
        
        introTransition.DoNextTransition(0);
    }
    public void StartGame()
    {
        runtimeChoiceManager.ResetRun();
        SceneManager.LoadSceneAsync(1);
    }

    public void OpenCredits()
    {
        creditsMenu.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
