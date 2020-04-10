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
    public int maxHp = 0;
    public int currentHp = 0;

    float damageScaleX = 5, currentHealthScaleX = 5;

    public ChoiceCategory runtimeChoices;
    public P1Stats playerRuntimeStats;


    public void GetCurrentHP(int currentHealth)
    {
        GetMaxHP();
        currentHp = currentHealth;
        VisualiseHealthChange(currentHp);
    }

    public void UpdateHPValues(int currentHealth, int maxHitPoints)
    {
        maxHp = maxHitPoints;
        currentHp = currentHealth;
        VisualiseHealthChange(currentHp);
    }


    public void GetMaxHP()
    {
        if (this.CompareTag("Monster"))
        {
            try
            {
                maxHp = GetComponentInParent<EnemyBehaviour>().agent.health;
            }
            catch
            {
                Debug.Log("Could not find the MaxHp of the Minion. Have you chosen one yet?");
            }
        }
        else if (this.CompareTag("P1"))
        {
            maxHp = playerRuntimeStats.maxHitPoints;
        }
    }


    public void VisualiseHealthChange(int currentHealth)
    {
        if (currentHealth > 0)
        {
            freezeVisibleDamage = true;
            timer = 0;
            hitPointsText.SetText(currentHealth + " HP");
            currentHealthScaleX = (5 * currentHealth / maxHp);
            CurrentHealthFillTransform.localScale = new Vector3(currentHealthScaleX, 1, 1);
        }
        else
        {
            freezeVisibleDamage = true;
            timer = 0;
            hitPointsText.SetText(0 + " HP");
            currentHealthScaleX = 0;
            CurrentHealthFillTransform.localScale = new Vector3(currentHealthScaleX, 1, 1);
        }


        // Debug.Log(HealthBarScaleTransform.localScale);

    }

    private void Update()
    {
        if (freezeVisibleDamage == true)
        {
            timer += Time.deltaTime;
            if (timer > freezeTime)
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
