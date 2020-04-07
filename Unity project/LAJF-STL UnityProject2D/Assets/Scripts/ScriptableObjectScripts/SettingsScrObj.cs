﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObject/GameSettings")]
public class SettingsScrObj : ScriptableObject
{
    [SerializeField]
    [Range(2, 4)]
    private int amountOfPlayers;

    public void IncrementNumberOfPlayers(int increment)
    {
        amountOfPlayers += increment;
        amountOfPlayers = Mathf.Clamp(amountOfPlayers, 2, 4);
    }

    public int GetAmountOfPlayers()
    {
        return amountOfPlayers;
    }
}