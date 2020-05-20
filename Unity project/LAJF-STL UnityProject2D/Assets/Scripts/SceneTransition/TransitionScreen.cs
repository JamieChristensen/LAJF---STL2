using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using STL2.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    private CanvasGroup _TransitionCanvasGroup;
    public Image transitionBackgroundPanel;
    public TextMeshProUGUI middleInfo;
    public TransitionElements[] transitionElements;
    private TransitionElements _nextTransitionElements;
    public VoidEvent readyToLoadScene, loadedAdditiveScene;

    int whichScene;
    public int voiceLineIndex;
    public AudioList audioList;

    public ChoiceCategory runtimeChoices;

    bool transitioning = false;
    Coroutine co;

    public string firstPart = "";

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

    public TransitionNarrator transitionNarrator;

    float introductionDelay;


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
        if (outOfThreeScenarios == 3)
        {
            atTransitionDestinationScene = true;
            DoNextTransition(15);
        }
        else if (outOfThreeScenarios == 2)
        {
            atTransitionDestinationScene = true;
            DoNextTransition(11);
        }

    }

    private void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            DoNextTransition(3);
        }

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

        if (SceneManager.sceneCount > 2)
        {
            List<int> sceneIndices = new List<int>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                sceneIndices.Add(SceneManager.GetSceneAt(i).buildIndex);
                switch (sceneIndices[i])
                {
                    case 5:
                        DoNextTransition(9); // transition to Minion scene
                        break;
                    case 6:
                        DoNextTransition(11); // transition to Modifier scene
                        break;
                    case 7:
                        DoNextTransition(15); // transition to Item scene
                        break;
                }
            }
            loadedAdditiveScene.Raise();
            // Debug.Log("Additive Scene has been loaded");
        }
    }

    public void MainMenuException()
    {
        Debug.Log("MainMenuException");
        atTransitionDestinationScene = false;
        introductionDelay = 4;
    }


    public void DoNextTransition(int transitionIndex)
    {
        whichScene = transitionIndex;

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
        yield return new WaitForSeconds(0.05f);
        audioList = FindObjectOfType<AudioList>();
        yield return new WaitForSeconds(introductionDelay);
        int fillerIndex = 0;
        transitioning = true;
        if (transitionElements[transitionIndex] != null)
        {
            if (transitionElements[transitionIndex].transitionBackgrounds.Count == 1)
            {
                if (transitionElements[transitionIndex].transitionBackgrounds[0] && transitionBackgroundPanel != null)
                {
                    transitionBackgroundPanel.sprite = transitionElements[transitionIndex].transitionBackgrounds[0];
                }

            }

            if (transitionElements[transitionIndex].transitionBackgrounds[0] == null)
            {

                for (int i = 0; i < runtimeChoices.chosenEnvironments.Length; i++)
                {
                    transitionElements[transitionIndex].environmentBackgrounds[i] = runtimeChoices.chosenEnvironments[i].environmentSprite;
                }

                transitionBackgroundPanel.sprite = transitionElements[transitionIndex].environmentBackgrounds[runtimeChoices.runTimeLoopCount - 1];
            }



            if (transitionIndex == 0)
            {
                fillerIndex = Random.Range(0, transitionElements[0].introFillers.Length);
                firstPart = transitionElements[0].introFillers[fillerIndex];
            }

            if (transitionIndex != 18)
            {
                middleInfo.text = firstPart + transitionElements[transitionIndex].textElement[0].textInputs[runtimeChoices.runTimeLoopCount - 1];
                audioList.narratorVoiceLines.clip = transitionElements[transitionIndex].transitionVoiceLines[runtimeChoices.runTimeLoopCount - 1];
            }
            else if (transitionIndex == 18)
            {

                bool didHeroWin = false;
                foreach (PlayerItems item in runtimeChoices.playerItems)
                {
                    didHeroWin = (item.itemName == "Victory Shades") ? true : didHeroWin;
                }
                if (didHeroWin)
                {
                    middleInfo.text = transitionElements[transitionIndex].textElement[0].textInputs[0];
                    transitionBackgroundPanel.sprite = transitionElements[transitionIndex].transitionBackgrounds[0];
                    audioList.narratorVoiceLines.clip = transitionElements[transitionIndex].transitionVoiceLines[0];
                }
                else
                {
                    middleInfo.text = transitionElements[transitionIndex].textElement[0].textInputs[1];
                    transitionBackgroundPanel.sprite = transitionElements[transitionIndex].transitionBackgrounds[1];
                    audioList.narratorVoiceLines.clip = transitionElements[transitionIndex].transitionVoiceLines[1];
                }

            }
            else
            {
                middleInfo.text = "";
            }

            if (atTransitionDestinationScene == false)
            {
                yield return new WaitForSeconds(0.5f);
                if (firstPart != "")
                {
                    transitionNarrator.DoVoiceFiller(fillerIndex);
                }
                yield return new WaitForSeconds(0.1f);
                transitionNarrator.DoNarration(middleInfo.text);
            }


        }



        _nextTransitionElements = transitionElements[transitionIndex];

        float timeToLerp = transitionElements[transitionIndex].textElement[0].timeOfTextDisplayed;
        float timeLerped = 0;

        //Debug.Log("Doing Transition - " + _nextTransitionElements.details);

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
            timeLerped += Time.fixedDeltaTime;
            _TransitionCanvasGroup.alpha = Mathf.Lerp(_TransitionCanvasGroup.alpha, oppositeAlpha, timeLerped / timeToLerp);

            if (oppositeAlpha == 1 && timeLerped > timeToLerp)
            {
                _TransitionCanvasGroup.alpha = 1;
                transitioning = false;
            }
            else if (oppositeAlpha == 0 && timeLerped > timeToLerp)
            {
                _TransitionCanvasGroup.alpha = 0;
                transitioning = false;

            }

            yield return new WaitForSeconds(0.02f);
        }

        while (audioList.narratorVoiceLines.isPlaying || audioList.textToSpeechSource.isPlaying)
        {
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1f);

        if (atTransitionDestinationScene)
        {
            atTransitionDestinationScene = false;
        }
        else if (transitionIndex == 18)
        {
            FindObjectOfType<CustomSceneManager>().LoadCredits();
        }
        else
        {

            readyToLoadScene.Raise();

        }




        //   Debug.Log("Done with Transition");

    }


    #endregion


}
