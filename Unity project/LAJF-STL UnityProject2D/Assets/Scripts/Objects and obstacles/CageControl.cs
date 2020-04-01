using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using STL2.Events;
public class CageControl : MonoBehaviour
{
    public Transform highTransform;
    private Vector3 _spawnPoint;
    private int timer = 3;
    public TextMeshProUGUI countDown;
    Coroutine co;
    bool runningCoroutine = false;
    public IntEvent transitionToMinionScene;


    public void OnStartCountDown()
    {
        InvokeRepeating("StartCountDown", 1, 1); // 
        countDown.text = "Ready?";
    }

    private void Awake()
    {
        _spawnPoint = transform.position;
        transform.position = highTransform.position;
    }

    private void Start()
    {
        CaptureHero();
    }


    public void CaptureHero()
    {
        if (runningCoroutine)
        {
            StopCoroutine(co);
            runningCoroutine = false;
        }
        co = StartCoroutine(CaptureHeroRoutine());
    }

    IEnumerator CaptureHeroRoutine()
    {
        
        runningCoroutine = true;
        Vector3 targetPosition = _spawnPoint;
        float timer = 0;
        while (transform.position != targetPosition && timer < 2)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.025f);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        runningCoroutine = false;
        
    }


    public void StartCountDown()
    {
        
        if (timer == 0)
        {
            CancelInvoke();
            if (runningCoroutine)
            {
                StopCoroutine(co);
                runningCoroutine = false;
            }
            co = StartCoroutine(ReleaseTheHero());
            countDown.text =  "Go!";
        }
        else if (timer > 0)
        {
            countDown.text = timer.ToString();
            timer--;
        }
        
    }


    IEnumerator ReleaseTheHero()
    {
        float timer = 0;
        runningCoroutine = true;
        Vector3 targetPosition = highTransform.position;

        while (transform.position != targetPosition && timer < 2)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition,0.025f);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        runningCoroutine = false;
    }


}
