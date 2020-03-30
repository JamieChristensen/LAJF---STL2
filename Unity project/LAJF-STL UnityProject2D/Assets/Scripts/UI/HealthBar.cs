using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Canvas healthbarCanvas;
    public TMP_Text hitPointsText;
    public Transform CurrentHealthFillTransform;
    public Transform DamageHealthFillTransform;

    bool freezeVisibleDamage = false;
    float freezeTime = 0.5f, timer = 0;
    public int maxHp = 30;

    float damageScaleX = 5, currentHealthScaleX = 5;


    public void VisualiseHealthChange(int currentHealth)
    {
        freezeVisibleDamage = true;
        timer = 0;
        hitPointsText.SetText(currentHealth + " HP");
        currentHealthScaleX = (5*currentHealth / maxHp );
        Debug.Log(currentHealth.ToString());
        Debug.Log(currentHealthScaleX.ToString());
        CurrentHealthFillTransform.localScale = new Vector3(currentHealthScaleX, 1,1);






       
       // Debug.Log(HealthBarScaleTransform.localScale);

    }

    private void Update()
    {
        if(freezeVisibleDamage == true)
        {
            timer += Time.deltaTime;
            if(timer > freezeTime)
            {
                freezeVisibleDamage = false;
            }
        }
        else
        {
            damageScaleX = Mathf.Lerp(damageScaleX, currentHealthScaleX, Time.deltaTime * 4f);
            DamageHealthFillTransform.localScale = new Vector3(damageScaleX, 1, 1);
        }
        

    }



}
