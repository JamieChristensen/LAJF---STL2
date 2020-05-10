using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class TransitionNarrator : MonoBehaviour
{
   // public AudioSource audioSource;
    public Camera mainCam;
    public AudioList audioList;


    public int getInSpeed = 5;
   // public Rigidbody2D rb2;
    public float readSpeed;
    public TextMeshProUGUI uiText; // the text to read
    public GameObject visibleNarratorGameobject;

    private AudioClip textToSpeechClip;

    public Vector2 previousPosition;

    public bool OnMainMenu = false;

    public TTS textToSpeech;

    bool realNarrator = true;



    private void Start()
    {
        visibleNarratorGameobject.SetActive(false);
        textToSpeech = FindObjectOfType<TTS>();
        if (mainCam == null)
        {
            mainCam = FindObjectOfType<Camera>();
        }
        
    }

    public void DoNarration(string readableText)
    {
        if (realNarrator)
        {
            DoVoiceLine();
            return;
        }
        Narrate(readableText);
        visibleNarratorGameobject.SetActive(true);
    }

    public void DoVoiceFiller(int fillerIndex)
    {
        audioList.narratorVoiceFillers[fillerIndex].Play();
    }

    public void DoVoiceLine()
    {
        bool fillerIsPlaying = false;
        AudioSource fillerSource = new AudioSource();
       foreach (AudioSource AS in audioList.narratorVoiceFillers)
       {
            if (AS.isPlaying)
            {
                fillerIsPlaying = true;
                fillerSource = AS;
                break;
            }
       }
       
        if (fillerIsPlaying)
        {
            StartCoroutine(WaitForFiller(fillerSource));
            return;
        }
        audioList.narratorVoiceLines.Stop();
       audioList.narratorVoiceLines.Play();
    }

    IEnumerator WaitForFiller(AudioSource fillerSource)
    {
        while (fillerSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        audioList.narratorVoiceLines.Play();
    }
    public void Narrate(string text) // call this with uiText as parameter
    {
        // TODO make set text string from somewhere else
        // string text = "The hero has slain the foul beast and is rewarded with a treasure!";

        StartCoroutine(ReadText(text));
    }


    IEnumerator ReadText(string text)
    {
        textToSpeechClip = audioList.textToSpeechSource.clip;
        // execute TTS service with text param and wait for respons 
        yield return StartCoroutine(textToSpeech.SynthesizeText(text, audioList));

        //isEnteringScene = true;
        //isExitingScene = false;

        /*
        // wait for narrator to clear his throat 
        yield return new WaitForSeconds(audioSource.clip.length);
        */

        // show text on screen 
        if (textToSpeechClip != null)
        {
            audioList.textToSpeechSource.Play();
        }
        uiText.text = text;
        // play audio file 

        yield return new WaitForSeconds(4 * readSpeed);
        //isEnteringScene = false;
        //isExitingScene = true;
        // remove text 
        uiText.text = "";
        //isRunning = false;
    }

}
