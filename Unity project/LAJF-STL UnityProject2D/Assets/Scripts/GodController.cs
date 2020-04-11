using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class GodController : MonoBehaviour
{
    [SerializeField]
    private ChoiceCategory runtimeChoices;
    [SerializeField]
    private GodInformation godInfo;

    [SerializeField]
    [Range(1, 3)]
    private int godNumber;

    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private TextMeshPro tmproName;

    private bool isEnabled;

    [SerializeField]
    private Sprite[] emotes;
    [SerializeField]
    private SpriteRenderer emoteRenderer;

    private float emoteDuration, emoteMaxTime;
    private bool isEmoting;

    [SerializeField]
    private SettingsScrObj gamesettings;

    //Lightning
    public KeyCode shoot;
    public GameObject lightningPrefab;
    public float lightningSpeed;
    public Transform firePoint;
    private bool OnCooldown = false;
    private float timer = 0, cooldownTime = 30;

    public void Start()
    {
        emoteDuration = 0;
        emoteMaxTime = 1f;
        isEmoting = false;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (runtimeChoices.chosenGods.Length >= godNumber && gamesettings.GetAmountOfPlayers() > godNumber)
        {
            godInfo = runtimeChoices.chosenGods[godNumber - 1];
            sprite = godInfo.topBarIcon;
            spriteRenderer.sprite = sprite;
            isEnabled = true;
            tmproName.text = godInfo.godName;
            Debug.Log("Finished god-initializing");
        }
        else
        {
            spriteRenderer.sprite = null;
            isEnabled = false;
            gameObject.SetActive(false);
        }

    }

    void Update()
    {

        if (Input.GetKeyDown(shoot) && OnCooldown == false)
        {
            Shoot();
            timer = 0;
            OnCooldown = true;
        }

        if (timer > cooldownTime)
        {
            OnCooldown = false;
        }


        if (OnCooldown)
        {
            cooldownTime += Time.deltaTime;
        }

        if (!isEmoting)
        {
            return;
        }

        emoteDuration += Time.deltaTime;
        if (emoteMaxTime <= emoteDuration)
        {
            isEmoting = false;
            emoteDuration = 0;
            emoteRenderer.sprite = null;
        }
    }

    public int GetGodNumber()
    {
        return godNumber;
    }

    public void Shoot()
    {
        Debug.Log("Lightning");
        GameObject instance = Instantiate(lightningPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.down * lightningSpeed, ForceMode2D.Impulse);
    }

    public void Emote()
    {
        emoteRenderer.sprite = emotes[UnityEngine.Random.Range(0, emotes.Length)];
        isEmoting = true;
        emoteDuration = 0;
    }
}