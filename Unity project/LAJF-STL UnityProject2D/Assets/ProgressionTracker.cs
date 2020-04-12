using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionTracker : MonoBehaviour
{
    public Sprite defeated;
    // public Sprite defeated;
    public Sprite noInfo;
    public GameObject[] levels;
    public ChoiceCategory runtimeChoices;
    public GameObject startProgression;

    private int progress = 1;

    public void UpdateProgression()
    {
        progress = runtimeChoices.runTimeLoopCount;

        ResetProgression();

        // looping over the levels that the hero have gone through
        for (int i = 1; i <= progress; i++)
        {
            // current encouonter 
            if (i == progress)
            {
                Sprite enemy = runtimeChoices.enemies[i - 1].sprite;

                // find all children gameobjects in the game
                foreach (Transform childTransform in levels[i - 1].GetComponentsInChildren<Transform>(true))
                {
                    GameObject child = childTransform.gameObject;
                    if (child.name == "Progression Arrow")
                        child.SetActive(true);
                    if (child.name == "Encounter")
                        child.GetComponent<Image>().sprite = enemy;
                }
            }
            else
            {
                
                // find the previous opponents
                Sprite enemy = runtimeChoices.enemies[i - 1].sprite;
                // add dead material or red cross when we find it;
                foreach (Transform childTransform in levels[i - 1].GetComponentsInChildren<Transform>(true))
                {
                    GameObject child = childTransform.gameObject;
                    if (child.name == "Encounter")
                    {
                        Image enemyImage = child.GetComponent<Image>();
                        enemyImage.sprite = enemy;
                    }
                    if (child.name == "Defeated")
                        child.SetActive(true);
                    
                }
                
            }

        }
    }

    private void ResetProgression()
    {
        // deactivate all elements
        foreach (GameObject level in levels)
        {
            foreach (Transform trans in level.GetComponentsInChildren<Transform>(true))
            {
                GameObject child = trans.gameObject;
                if (child.name == "Progression Arrow")
                    child.SetActive(false);
                if (child.name == "Encounter")
                    child.GetComponent<Image>().sprite = noInfo;
                if (child.name == "Defeated")
                    child.SetActive(false);
            }
        }

    }

    public void SetNewProgression()
    {
        ResetProgression();
        startProgression.SetActive(true);
    }
}
