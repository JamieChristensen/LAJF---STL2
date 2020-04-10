using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using STL2.Events;
public class CageControl : MonoBehaviour
{
    public Transform highTransform;
    private Transform _playerTransform;
    private Vector3 _spawnPoint, _currentPosition;
 
    private int timer = 3;
    public TextMeshProUGUI countDown;
    Coroutine co;
    bool runningCoroutine = false;
    
    public VoidEvent heroHasBeenCaptured;

    private MusicManager _musicManager;
    public MusicManager musicManager { get { return _musicManager; } }

    private void Awake()
    {
        _playerTransform = GameObject.Find("Player1").GetComponent<Transform>();
        _spawnPoint = transform.position;
        transform.position = highTransform.position;
    }

    private void Start()
    {
        _musicManager = FindObjectOfType<MusicManager>();
        CaptureHero();
    }


    public void CaptureHero()
    {
        try
        {
            musicManager.adjustCurrentPlayingVolume(0.5f);
        }
        catch
        {
           // Debug.Log("there is no music manager");
        }
        
        if (runningCoroutine)
        {
            StopCoroutine(co);
            runningCoroutine = false;
        }
        co = StartCoroutine(CaptureHeroRoutine());
    }

    IEnumerator CaptureHeroRoutine()
    {
        _currentPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z );
        yield return new WaitForSeconds(2f);
        runningCoroutine = true;
        Vector3 targetPosition = _spawnPoint;
        float timer = 0;
        while (transform.position != targetPosition && timer < 10)
        {  
            targetPosition = new Vector3 (_playerTransform.position.x,_spawnPoint.y,_spawnPoint.z); 
            transform.position = Vector3.Lerp(transform.position, targetPosition, timer/ 2000f);
            _currentPosition = new Vector3 (_playerTransform.position.x,transform.position.y,transform.position.z);
            transform.position = _currentPosition;
            timer += Time.fixedDeltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        timer = 0;
        while (transform.position != targetPosition && timer < 1)
        {
            targetPosition = new Vector3(_playerTransform.position.x, _spawnPoint.y, _spawnPoint.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, timer / 2f);
            _currentPosition = new Vector3(_playerTransform.position.x, transform.position.y, transform.position.z);
            transform.position = _currentPosition;
            timer += Time.fixedDeltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2f);
        runningCoroutine = false;
        heroHasBeenCaptured.Raise(); // Telling the game that the Hero has been captured
        yield return new WaitForSeconds(1f);
        try
        {
            musicManager.adjustCurrentPlayingVolume(1);
        }
        catch
        {
           // Debug.Log("there is no music manager");
        }
    }


    public void OnStartCountDown()
    {
        InvokeRepeating("StartCountDown", 5, 1); // 
        countDown.text = "Ready?";
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

        while (transform.position != targetPosition && timer < 5)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, timer / 100f);
            timer += Time.fixedDeltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        runningCoroutine = false;
    }


}
