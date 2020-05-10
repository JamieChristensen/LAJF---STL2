using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObject/GameSettings")]
public class SettingsScrObj : ScriptableObject
{
    [SerializeField]
    [Range(2, 4)]
    private int amountOfPlayers;

    [Range(0.0001f, 1)]
    public float gameMasterVolume, gameMusicVolume, gameSFXVolume;

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
