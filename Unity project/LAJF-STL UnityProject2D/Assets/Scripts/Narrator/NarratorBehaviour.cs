using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NarratorBehaviour : MonoBehaviour
{
    public AudioList audioList;
    public AudioClip clearThroat;
    public AudioClip textToSpeechClip;

    public Camera mainCam;
    public int getInSpeed = 4;
    public Rigidbody2D rb2;
    public float readSpeed;
    public TextMeshProUGUI uiText;

    public Vector2 targetPosition;
    public Vector2 previousPosition;

    [SerializeField]
    //private ChoiceCategory runTimeChoices;
    private TTS textToSpeech;

    private int uiOffset = -17; // to make up for the bottom UI.
    private bool isEnteringScene;
    private bool isExitingScene;
    private bool isRunning;

    void Start()
    {
        audioList = FindObjectOfType<AudioList>();
        textToSpeech = FindObjectOfType<TTS>();
        StartCoroutine(textToSpeech.InitalizeService());
        uiText.text = "";
    }
    private void Update()
    {
        // 
        if (isEnteringScene)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, getInSpeed*Time.deltaTime);
        }
        if (isExitingScene)
        {
            transform.position = Vector2.MoveTowards(transform.position, previousPosition, getInSpeed * Time.deltaTime);
        }
        
        
    }   

    void RandomizePosition()
    {
        float y, x;

        bool isVertical = UnityEngine.Random.Range(0f, 1f) >= 0.5f; // whether to spawn on a vertical or horizontal side.
        bool switchSide = UnityEngine.Random.Range(0f, 1f) >= 0.5f; // determine which side to use / width or height

        if (isVertical)
        {
            y = UnityEngine.Random.Range(mainCam.ScreenToWorldPoint(new Vector3(0, 0, 1f)).y, mainCam.ScreenToWorldPoint(new Vector3(0, mainCam.pixelHeight, 1f)).y);
            if (switchSide)
                x = mainCam.ScreenToWorldPoint(new Vector3(0, 0, 1f)).x;
            else
                x = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth, 0, 1f)).x;

        }
        else
        {
            if (switchSide)
                y = mainCam.ScreenToWorldPoint(new Vector3(0, 0, 1f)).y;
            else
                y = mainCam.ScreenToWorldPoint(new Vector3(0, mainCam.pixelHeight, 1f)).y;

            x = UnityEngine.Random.Range(mainCam.ScreenToWorldPoint(new Vector3(0, 0, 1f)).x, mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth, 0, 1f)).x);
        }

        // making sure narrator isn't in a wrong spot  
        if (y < uiOffset)
        {
            y = uiOffset;
        }
        rb2.velocity = Vector2.zero; //Reset velocity to prevent flying away instantly if previously hit by chest.
        rb2.angularVelocity = 0;
        Vector3 newPos = new Vector3(x, y);
        transform.position = newPos;
        previousPosition = newPos;
        targetPosition = Vector2.Lerp(newPos, mainCam.transform.position, 0.2f);

    }

    public void Narrate(string text) // this function is to be utilized elsewhere
    {
        if (isRunning)
        {
            return;
        }
        isRunning = true;
        Debug.Log("Trying to read this: " + text);
        StartCoroutine(ReadText(text));
        
    }

    IEnumerator ReadText(string text)
    {
        yield return new WaitForSeconds(2);
        // execute TTS service with text param and wait for respons 
        yield return StartCoroutine(textToSpeech.SynthesizeText(text, this));
        
        RandomizePosition();
        int randomSource = UnityEngine.Random.Range(0, 3);
        audioList.narratorEnter[randomSource].Play();

        isEnteringScene = true;
        isExitingScene = false;

        // wait for narrator to clear his throat 
        yield return new WaitForSeconds(audioList.narratorEnter[randomSource].clip.length);
        
        
        // show text on screen 
        if (textToSpeechClip != null)
        {
            audioList.textToSpeechSource.clip = textToSpeechClip;
            if (!audioList.narratorHit.isPlaying)
            {
                audioList.textToSpeechSource.Play();
            }
            
        }

        audioList.AnnounceEnemyDeath();

        uiText.text = text;
        // play audio file 

        yield return new WaitForSeconds(4 * readSpeed);
        isEnteringScene = false;
        isExitingScene = true;
        // remove text 
        uiText.text = "";
        isRunning = false;
    }
}
