using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectMainMenuButton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            GameObject.Find("Main Menu button").GetComponent<Button>().Select();
        }
    }
}
