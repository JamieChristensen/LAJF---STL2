using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerAmountSelection : MonoBehaviour
{
    public SettingsScrObj settings;
    [SerializeField]
    private TextMeshProUGUI playerCountTMPro;
    private string baseText = " Players";

    void Start()
    {
        DisplayAmountOfPlayers();
    }

    public void DisplayAmountOfPlayers()
    {
        playerCountTMPro.text = settings.GetAmountOfPlayers().ToString() + baseText;
    }

    public void IncrementPlayerCount(int increment)
    {
        settings.IncrementNumberOfPlayers(increment);
        DisplayAmountOfPlayers();
    }
}
