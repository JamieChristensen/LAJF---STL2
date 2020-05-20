using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialPanels;
    int panelIndex;

    public void SwitchPanel(int addition)
    {
        panelIndex += addition;

        if (panelIndex == tutorialPanels.Length)
            panelIndex = 0;
        else if (panelIndex < 0)
            panelIndex = tutorialPanels.Length - 1;

        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            if (i == panelIndex)
            {
                tutorialPanels[i].gameObject.SetActive(true);
            }
            else
            {
                tutorialPanels[i].gameObject.SetActive(false);
            }
        }

    }


}
