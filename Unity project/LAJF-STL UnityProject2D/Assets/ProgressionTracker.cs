using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionTracker : MonoBehaviour
{
    public Material defeated;
    // public Sprite defeated;
    public Sprite noInfo;
    public GameObject[] levels;
    public ChoiceCategory runtimeChoices;
    public GameObject startProgression;

    private int progress = 1;

    public void UpdateProgression()
    {
        progress = runtimeChoices.runTimeLoopCount;
        Debug.Log("updating, progress is " + progress);

        ResetProgression();

        // looping over the levels that the hero have gone through
        for (int i = 1; i <= progress; i++)
        {
            if(i == progress)
            {
                Sprite enemy = runtimeChoices.enemies[runtimeChoices.enemies.Count - 1].sprite;
                // find all children gameobjects in the game
                foreach (Transform childTransform in levels[i - 1].transform)
                {
                    GameObject child = levels[i - 1].GetComponentInChildren<Transform>().gameObject;
                    if (child.name == "progression Arrow")
                        gameObject.SetActive(true);
                    if (child.name == "Encounter")
                        gameObject.GetComponent<Image>().sprite = enemy;
                }
            }
            
            else
            {
                // find the previous opponents
                Sprite enemy = runtimeChoices.enemies[i - 1].sprite;
                // add dead material or red cross when we find it;
                foreach (Transform childTransform in levels[i - 1].transform)
                {
                    GameObject child = levels[i - 1].GetComponentInChildren<Transform>().gameObject;
                    if (child.name == "Encounter")
                    {
                        Image enemyImage = gameObject.GetComponent<Image>();
                        enemyImage.sprite = enemy;
                        enemyImage.material = defeated;
                    }
                }
            }

        }
    }

     private void ResetProgression()
    {
        // deactivate all elements
        foreach(GameObject level in levels)
        {
            foreach(Transform trans in transform)
            {
                GameObject child = trans.gameObject;
            if (child.name == "progression Arrow")
                gameObject.SetActive(false);
            if (child.name == "Encounter")
                gameObject.GetComponent<Image>().sprite = noInfo;
            }
        }
        
    }

       public void SetNewProgression()
    {
        ResetProgression();
        startProgression.SetActive(true);
    }
}
