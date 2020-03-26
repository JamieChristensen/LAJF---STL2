using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


    public class NarratorBehaviour : MonoBehaviour
    {
    public AudioSource audio;
    public Camera mainCam;
    public int getInSpeed = 5;
    public Rigidbody2D rb2;
    public float readSpeed;
    public TextMeshProUGUI uiText;
    private float timer;
    private bool hasFired = false; 

    public Vector2 previousPosition;

    void Start()
        {
            uiText.text = "";
        timer = 0;
        }




    // Update is called once per frame
    void Update()
        {
        timer += Time.deltaTime;
//            Debug.Log(timer);
        if (timer > 6 && !hasFired)
        {
            hasFired = true;
            StartCoroutine(readText("din mor"));
        }
        if (Input.GetMouseButtonUp(2))
            RandomizePosition();
        }

    private void exitScene()
    {
        Vector2 direction = - (mainCam.transform.position - transform.position).normalized;
        rb2.velocity = direction * getInSpeed;
    }

    void EnterScene()
    {
        //RandomizePosition();
        Vector2 direction = (mainCam.transform.position - transform.position).normalized;
        rb2.velocity = direction*getInSpeed;
        audio.Play();
    }

    void RandomizePosition()
    {
        float y, x;
        //determine which side to use / width or height
        if (UnityEngine.Random.Range(0,1) >= 0.5f)
        {
            y = UnityEngine.Random.Range(0, mainCam.scaledPixelWidth);
            x = 0;
        }
        else{
            y = 0;
            x = UnityEngine.Random.Range(0, mainCam.scaledPixelWidth);
        }
        Vector3 newPos = new Vector3(x, y);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 100);
    }

    IEnumerator readText(string text)
    {
        // execute TTS service with text param and wait for respons 

      //  RandomizePosition();
        EnterScene();
        
        // show text on screen 
        uiText.text = text;
        
        yield return new WaitForSeconds(4*readSpeed);
        exitScene();
        uiText.text = "";
        // remove text 
    } 
}
     