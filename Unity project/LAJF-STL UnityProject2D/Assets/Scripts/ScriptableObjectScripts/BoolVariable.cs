using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Variable/BoolVar")]
public class BoolVariable : ScriptableObject
{
    //[SerializeField] private bool isSetting;

    public bool myBool;

    public bool GetBool()
    {
        return myBool;
    }

    public void setBool(bool _bool)
    {
        myBool = _bool;
    }
}
