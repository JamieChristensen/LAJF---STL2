using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Button selectedButton;
    public Button startGameButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button exitGameButton;

    public Button[] primaryMainMenuButtons;

    public GameObject settingsMenu;
    public GameObject creditsMenu;

    public TransitionScreen introTransition;
    public RuntimeChoiceManager runtimeChoiceManager;

    bool notFaded = true;

    private void Start()
    {

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            startGameButton.Select();
            print("selected button");
        }
    }
    private void Update()
    {

        if (Input.GetAxis("Vertical") != 0)
        {
            if (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.activeSelf == false)
            {
                startGameButton.Select();
            }
        }
    }

    public void DisablePrimaryButtons()
    {
        foreach (Button button in primaryMainMenuButtons)
        {
            button.interactable = false;
        }
    }

    public void EnablePrimaryButtons()
    {
        foreach (Button button in primaryMainMenuButtons)
        {
            button.interactable = true;
        }
    }

    //TODO: Make settings interactable. 

    public void StartFading()
    {
        if (notFaded)
        {
            StartCoroutine(DelayedTransition(1.5f));
            notFaded = false;
        }

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

    IEnumerator DelayedTransition(float delay)
    {
        yield return new WaitForSeconds(delay);
        introTransition.DoNextTransition(0);
    }
}
