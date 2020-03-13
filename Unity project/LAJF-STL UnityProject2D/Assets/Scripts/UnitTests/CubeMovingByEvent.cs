using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovingByEvent : MonoBehaviour
{
    //Not called anywhere except from int-event being raised (without reference to the script that raises the event!)
   public void SetCubePosition(int xCoord){
       transform.position = new Vector3(transform.position.x + xCoord, transform.position.y, transform.position.z);
   }
}
