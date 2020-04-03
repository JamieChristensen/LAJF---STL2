﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using STL2.Events;
using UnityEngine.SceneManagement;

public class TransitionScreen : MonoBehaviour
{
    private CanvasGroup _TransitionCanvasGroup;
    public TextMeshProUGUI middleInfo;
    public TransitionElements[] transitionElements;
    private TransitionElements _nextTransitionElements;
    public VoidEvent readyToLoadScene, loadedAdditiveScene;

    bool transitioning = false;
    Coroutine co;

    // time to fade on
    [SerializeField]
    private float _fadeOnDuration = 2f;
    public float FadeOnDuration { get { return _fadeOnDuration; } }

    // time to fade off
    [SerializeField]
    private float _fadeOffDuration = 2f;
    public float FadeOffDuration { get { return _fadeOffDuration; } }

    public bool atTransitionDestinationScene = true;

    int outOfThreeScenarios = 1;

    private void Awake()
    {
        _TransitionCanvasGroup = GetComponent<CanvasGroup>();

    }

    private void OnDisable()
    {
        outOfThreeScenarios++;
        if (outOfThreeScenarios > 3)
        {
            outOfThreeScenarios = 1;
        }
    }
    private void OnEnable()
    {
        if (outOfThreeScenarios == 2)
        {
            DoNextTransition(11);
        }
        else if (outOfThreeScenarios == 3)
        {
            DoNextTransition(15);
        }
    }

    private void Start()
    {

        if (SceneManager.sceneCount != 1)
        {

            if (gameObject.GetComponent<Canvas>().worldCamera == null)
            {
                try
                {
                    gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
                }
                catch
                {
                    Debug.Log("Camera could not be located in the scene!");
                }

            }
        }

        if (SceneManager.sceneCount > 1)
        {
            List<int> sceneIndices = new List<int>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                sceneIndices.Add(SceneManager.GetSceneAt(i).buildIndex);
                switch (sceneIndices[i])
                {
                    case 4:
                    DoNextTransition(7);
                    break;
                    case 5:
                    DoNextTransition(9);
                    break;
                    case 6:
                    DoNextTransition(13);
                    break;
                }
            }
            loadedAdditiveScene.Raise();
            // Debug.Log("Additive Scene has been loaded");
        }
    }

    public void MainMenuException()
    {
        atTransitionDestinationScene = false;
    }


    public void DoNextTransition(int transitionIndex)
    {
        if (transitioning)
        {
            StopCoroutine(co);
            atTransitionDestinationScene = false;
            transitioning = false;
        }


        co = StartCoroutine(Transition(transitionIndex));
    }


    #region Transition
    IEnumerator Transition(int transitionIndex)
    {
        transitioning = true;
        middleInfo.text = transitionElements[transitionIndex].textElement[0].textInput;
        _nextTransitionElements = transitionElements[transitionIndex];

        Debug.Log("Doing Transition - " + _nextTransitionElements.details);

        float oppositeAlpha;
        if (_nextTransitionElements.startTransparent)
        {
            if (_TransitionCanvasGroup.alpha == 1)
            {
                _TransitionCanvasGroup.alpha = 0;
            }

            oppositeAlpha = 1;
        }
        else
        {
            if (_TransitionCanvasGroup.alpha == 0)
            {
                _TransitionCanvasGroup.alpha = 1;
            }
            oppositeAlpha = 0;
        }



        while (transitioning)
        {
            _TransitionCanvasGroup.alpha = Mathf.Lerp(_TransitionCanvasGroup.alpha, oppositeAlpha, 0.05f);

            if (oppositeAlpha == 1 && _TransitionCanvasGroup.alpha > 0.99f)
            {
                _TransitionCanvasGroup.alpha = 1;
                transitioning = false;
            }
            else if (oppositeAlpha == 0 && _TransitionCanvasGroup.alpha < 0.01)
            {
                _TransitionCanvasGroup.alpha = 0;
                transitioning = false;

            }

            yield return new WaitForSeconds(0.02f);
        }

        if (atTransitionDestinationScene)
        {
            atTransitionDestinationScene = false;
        }
        else
        {

            readyToLoadScene.Raise();

        }

        Debug.Log("Done with Transition");

    }


    #endregion


}