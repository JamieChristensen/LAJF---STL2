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
        HealthBarScaleTransform.localScale.x = (maxHp / 100) * currentHealth;

    }



}
