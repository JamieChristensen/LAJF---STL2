using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using STL2.Events;

public class TransitionScreen : MonoBehaviour
{
    private CanvasGroup _TransitionCanvasGroup;
    public TextMeshProUGUI middleInfo;
    public TransitionElements[] transitionElements;
    private TransitionElements _nextTransitionElements;
    public VoidEvent readyToLoadScene;

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

    private void Awake()
    {
        _TransitionCanvasGroup = GetComponent<CanvasGroup>();
        
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

        if(atTransitionDestinationScene)
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
