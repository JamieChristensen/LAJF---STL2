using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Environmentpool", menuName = "ScriptableObject/Environmentpool")]
public class EnvironmentPool : ScriptableObject
{
    [Header("Environment Pool")]
    public List<Environments> environmentPool; 


}

[Serializable]
public class Environment
{
    public string environmentName;
    public string theme;
    public Sprite environmentSprite;
    public int environmentIndex;
}

[Serializable]

public class  Environments
{
    public string theme;
    public Environment[] environments;
}

