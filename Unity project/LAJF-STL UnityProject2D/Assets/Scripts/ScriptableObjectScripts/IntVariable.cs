using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Variable/IntVar")]
public class IntVariable : ScriptableObject
{
    //[SerializeField] private bool isSetting;

    public int myInt;

    public int GetInt()
    {
        return myInt;
    }

    public void setInt(int _int)
    {
        myInt = _int;
    }
}
