using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class NarratorBehaviour : MonoBehaviour
    {
    public AudioSource audio;
    public Camera mainCam;
    public int getInSpeed = 5;
    public Rigidbody2D rb2;

    public Vector2 previousPosition;
    


        void Start()
        {
        }




    // Update is called once per frame
    void Update()
        {

            if(Input.GetMouseButtonUp(0)){

                EnterScene();
                
            }

            if (Input.GetMouseButtonUp(1))
            {
                exitScene();
            }

            if (Input.GetMouseButtonUp(2))
            {
            Debug.Log("PRESSEd");
                RandomizePosition();
            }
            
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
            y = UnityEngine.Random.Range(0, mainCam.scaledPixelHeight);
            x = 0;
        }
        else{
            y = 0;
            x = UnityEngine.Random.Range(0, mainCam.scaledPixelWidth);
        }
        Vector3 newPos = new Vector3(x, y);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 100);
    }

           
       

    }

