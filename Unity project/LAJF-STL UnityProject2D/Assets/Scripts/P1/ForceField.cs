using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    Vector3 currentSize, targetSize;
    float timer, timeToLerp;
    public void ForceFieldToggle(bool active)
    {
        StartCoroutine(ToggleForceFieldAnimation(active));
    }

    IEnumerator ToggleForceFieldAnimation(bool active)
    {
        switch (active)
        {
            case true:
                currentSize = Vector3.zero; // start the forcefield size at 0
                targetSize = new Vector3(30, 30, 1); // set the target size to the one from the prefab
                timer = 0;
                timeToLerp = 0.3f;
                //scaling up
                while (timer < timeToLerp)
                {
                    timer += Time.deltaTime;
                    currentSize = Vector3.Lerp(currentSize, targetSize, 2 * Time.deltaTime / timeToLerp);
                    transform.localScale = currentSize;
                    yield return null;
                }
               
                break;

            case false:
                currentSize = targetSize; // start the forcefield size at the previous target Size
                targetSize = Vector3.zero; // set the target size to Vector3.zero
                timer = 0;
                timeToLerp = 0.3f;
                yield return new WaitForSeconds(0.2f);
                //scaling down
                while (timer < timeToLerp)
                {
                    timer += Time.deltaTime;
                    currentSize = Vector3.Lerp(currentSize, targetSize, 2 * Time.deltaTime / timeToLerp);
                    transform.localScale = currentSize;
                    yield return null;
                }
                Destroy(gameObject);
                break;
        }

        yield return null;
    }

}