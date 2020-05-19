using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour, IPausable
{
    public TextMeshProUGUI timerText;
    public const int COUNT_DOWN_TIME = 45;
    public bool isPaused;
    public STL2.Events.VoidEvent timesOut;
    
    private float timer;
    public bool isRunning;

    void Start()
    {
        timer = COUNT_DOWN_TIME;
        isRunning = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPaused || !isRunning)
            return;

        timerText.text = Mathf.Round(timer).ToString();

        if (timer <= 10)
            timerText.color = Color.red;

        if (timer <= 0)
        {
            isRunning = false;
            timerText.text = "00:00";
            timesOut.Raise();
        }

        timer -= Time.deltaTime;
    }
    // other scripts can call this script  
    public void StartTimerWithTime(int time = COUNT_DOWN_TIME)
    {
        timer = time;
        isRunning = true;
    }

    public bool IsPaused()
    {
        return isPaused == true;

    }

    public void Pause()
    {
        isPaused = true;
    }

    public void UnPause()
    {
        isPaused = false;
    }
}
