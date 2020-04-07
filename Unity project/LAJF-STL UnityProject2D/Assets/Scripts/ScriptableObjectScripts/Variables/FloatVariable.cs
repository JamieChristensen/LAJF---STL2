using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Variable/FloatVar")]

public class FloatVariable : ScriptableObject
{
    [SerializeField] private float myfloat;

    public void setValue(float value)
    {
        myfloat = value;
    }

    public float getValue()
    {
        return myfloat;
    }
}
