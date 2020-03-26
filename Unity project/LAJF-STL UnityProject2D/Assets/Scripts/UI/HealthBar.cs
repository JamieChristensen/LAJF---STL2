using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Canvas healthbarCanvas;
    public TMP_Text hitPointsText;
    public Transform HealthBarScaleTransform;
    public int maxHp;


    public void VisualiseHealthChange(int currentHealth)
    {

        hitPointsText.SetText(currentHealth + " HP");
        HealthBarScaleTransform.localScale = new Vector3(((maxHp / 100) * currentHealth)/20, 1,1);

        Debug.Log(currentHealth);
       // Debug.Log(HealthBarScaleTransform.localScale);

    }



}
