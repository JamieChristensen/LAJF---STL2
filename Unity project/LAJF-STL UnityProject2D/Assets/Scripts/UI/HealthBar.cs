using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;


public class HealthBar : MonoBehaviour
{
    public Canvas healthbarCanvas;
    public TMP_Text hitPointsText;
    public Transform CurrentHealthFillTransform;
    public Transform DamageHealthFillTransform;

    bool freezeVisibleDamage = false;
    float freezeTime = 0.5f, timer = 0;
    public float maxHp = 0;
    public float currentHp = 0;

    float damageScaleX = 5, currentHealthScaleX = 5;

    public ChoiceCategory runtimeChoices;
    public P1Stats playerRuntimeStats;

    [Header("Fade/Dissolve references")]
    public DissolveInformation[] thingsToDissolve;

    private bool fading = false;
    public float scaleOfFadeNoise;
    public Light2D light2d;
    public GameObject dissolveParticles; //assign in inspector

    private void Start()
    {
        light2d = transform.GetComponentInChildren<Light2D>();
    }

    public void GetCurrentHP(int currentHealth)
    {
        GetMaxHP();
        currentHp = currentHealth;
        VisualiseHealthChange(currentHealth);
    }

    public void UpdateHPValues(int currentHealth, int maxHitPoints)
    {
        maxHp = maxHitPoints;
        //Debug.Log("maxHp: " + maxHp);
        currentHp = currentHealth;
        //Debug.Log("currentHp: " + currentHp);
        VisualiseHealthChange(currentHealth);
    }


    public void GetMaxHP()
    {
        if (this.CompareTag("Monster"))
        {
            try
            {
                maxHp = GetComponentInParent<EnemyBehaviour>().maxHealth;
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
        currentHp = currentHealth;
        if (currentHp > 0)
        {
            freezeVisibleDamage = true;
            timer = 0;
            hitPointsText.SetText(currentHp + " HP");
            currentHealthScaleX = (5 * currentHp / maxHp);
            CurrentHealthFillTransform.localScale = new Vector3(currentHealthScaleX, 1, 1);
        }
        else
        {
            freezeVisibleDamage = true;
            timer = 0;
            hitPointsText.SetText(0 + " HP");
            currentHealthScaleX = 0;
            CurrentHealthFillTransform.localScale = new Vector3(currentHealthScaleX, 1, 1);

            //TODO: Start coroutines dissolving the HP-bars.

            if (!fading)
            {
                Destroy(hitPointsText.gameObject);
                fading = true;
                StartFade();
            }
        }


        //Debug.Log("CurrentHealthFillTransform.localScale: " + CurrentHealthFillTransform.localScale);

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
            if (CurrentHealthFillTransform.localScale == DamageHealthFillTransform.localScale)
            {
                return;
            }
            damageScaleX = Mathf.Lerp(damageScaleX, currentHealthScaleX, Time.deltaTime * 4f);
            DamageHealthFillTransform.localScale = new Vector3(damageScaleX, 1, 1);
            //Debug.Log("DamageHealthFillTransform.localScale: " + DamageHealthFillTransform.localScale);
        }
    }

    public void StartFade()
    {
        float maxDuration = 2;

        GameObject go = Instantiate(dissolveParticles, dissolveParticles.transform.position, Quaternion.identity);
        go.SetActive(true);
        Destroy(go, 7f); //The time that it takes the last particles to dissipate.
        foreach (DissolveInformation disInfo in thingsToDissolve)
        {
            if (disInfo.fadeDuration > maxDuration)
            {
                maxDuration = disInfo.fadeDuration;
            }
            StartCoroutine(Fade(disInfo.fadeDuration, disInfo.dissolveColor, disInfo.spriteRenderer));
        }
        light2d.intensity = 0;
        Destroy(gameObject, maxDuration);

    }

    IEnumerator Fade(float duration, Color color, SpriteRenderer spriteRenderer)
    {
        float timer = duration;
        Material mat = spriteRenderer.material;
        mat.SetColor("_Color", color);
        mat.SetFloat("_Scale", scaleOfFadeNoise);

        float maxLightIntensity = light2d.intensity;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            float fade = map(timer, duration, 0, 1, 0);
            Debug.Log("Fade: " + fade + "\n Time: " + timer);
            mat.SetFloat("_Scale", scaleOfFadeNoise);



            mat.SetFloat("_Fade", fade);
            yield return new WaitForSeconds(0);
        }
        yield return null;
    }

    [System.Serializable]
    public class DissolveInformation
    {

        [ColorUsage(true, true)]
        public Color dissolveColor;
        public SpriteRenderer spriteRenderer;

        public float fadeDuration;
    }


    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
