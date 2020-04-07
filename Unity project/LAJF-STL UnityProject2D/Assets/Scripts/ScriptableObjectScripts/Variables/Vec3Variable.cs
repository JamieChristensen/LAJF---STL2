using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Variable/Vector3")]
public class Vec3Variable : ScriptableObject
{
    //[SerializeField] private bool isSetting;

    public Vector3 myVector;

    public Vector3 GetVector3()
    {
        return myVector;
    }

    public void setInt(Vector3 _vector)
    {
        myVector = _vector;
    }
}
