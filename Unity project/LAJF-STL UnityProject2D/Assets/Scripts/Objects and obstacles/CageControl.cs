using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CageControl : MonoBehaviour
{
    public Transform highTransform;
    private int timer = 3;
    public TextMeshProUGUI countDown;
    public void OnStartCountDown()
    {
        InvokeRepeating("StartCountDown", 1, 1); // 
        countDown.text = "Ready?";
    }

    public void StartCountDown()
    {
        if (timer == 0)
        {
            CancelInvoke();
            StartCoroutine(ReleaseTheHero());
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
        Vector3 targetPosition = highTransform.position;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition,0.05f);
            yield return new WaitForSeconds(0.02f);
        }
        
    }


}
