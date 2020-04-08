using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TransitionNarrator : MonoBehaviour
{
    public AudioSource audioSource;
    public Camera mainCam;

    public int getInSpeed = 5;
   // public Rigidbody2D rb2;
    public float readSpeed;
    public TextMeshProUGUI uiText; // the text to read

    public Vector2 previousPosition;


    public void Start()
    {
        if (mainCam == null)
        {
            mainCam = FindObjectOfType<Camera>();
        }
        if (audioSource == null)
        {
            audioSource = FindObjectOfType<AudioList>().narratorRead;
        }
    }

    public void DoNarration()
    {
        Narrate(uiText.text);
    }

    public void Narrate(string text) // call this with uiText as parameter
    {
        // TODO make set text string from somewhere else
        // string text = "The hero has slain the foul beast and is rewarded with a treasure!";

        StartCoroutine(ReadText(text));
    }


    IEnumerator ReadText(string text)
    {
        // execute TTS service with text param and wait for respons 


        //  RandomizePosition();
        EnterScene();

        // play audio file 

        yield return new WaitForSeconds(4 * readSpeed);
        ExitScene();
        // remove text 
    }



    private void ExitScene()
    {
         Vector2 direction = -(mainCam.transform.position - transform.position).normalized;
        // rb2.velocity = direction * (getInSpeed + 5);
    }

    void EnterScene()
    {
        RandomizePosition();

         Vector2 direction = (mainCam.transform.position - transform.position).normalized;
        //rb2.velocity = direction * getInSpeed;
        audioSource.Play();
    }

    void RandomizePosition()
    {
        float y, x;
        // determine which side to use / width or height
        // could use while loop to check if position is allowed 

        bool bottomOrRight = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
        if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
        {
            y = UnityEngine.Random.Range(mainCam.ScreenToWorldPoint(new Vector3(0, 0, 1f)).y, mainCam.ScreenToWorldPoint(new Vector3(0, mainCam.pixelHeight, 1f)).y);
            if (bottomOrRight)
                x = mainCam.ScreenToWorldPoint(new Vector3(0, 0, 1f)).x;
            else
                x = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth, 0, 1f)).x;

        }
        else
        {

            if (bottomOrRight)
                y = mainCam.ScreenToWorldPoint(new Vector3(0, 0, 1f)).y;
            else
                y = mainCam.ScreenToWorldPoint(new Vector3(0, mainCam.pixelHeight, 1f)).y;

            x = UnityEngine.Random.Range(mainCam.ScreenToWorldPoint(new Vector3(0, 0, 1f)).x, mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth, 0, 1f)).x);
        }
        Vector3 newPos = new Vector3(x, y);
        Debug.Log(newPos);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 100);
    }

}
