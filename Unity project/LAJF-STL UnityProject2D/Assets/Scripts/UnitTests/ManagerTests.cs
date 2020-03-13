using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class ManagerTests : MonoBehaviour
{
    public IntEvent intEventTest;   //Assign in inspector
    public VoidEvent voidEventTest; //Assign in inspector

    public VoidTypeListener voidEventTestListener;
    public IntTypeListener playerHPTestListener; //Also assign in inspector - could be on any object, but is put in here for demonstration.
    
    [SerializeField]
    private float timer, maxTime; //Used for demonstrating timing without using satanic Enumerators.
    [SerializeField]
    private bool hasTimerRunOut = false;
    public void Start(){
        TestEvents();
        timer = 0;
        maxTime = 5;
        hasTimerRunOut = false;
    }

    public void Update(){
        if (hasTimerRunOut) {return;}
        if (timer > maxTime){
            Debug.Log("Timer ran out after " + timer + " seconds");
            hasTimerRunOut = true;

            //Demonstration of using an intEvent to tell another script that a timer has run out and pass a value:
            intEventTest.Raise((int)timer);
            return;
        }
        timer += Time.deltaTime;
        }
    

    public void TestEvents(){
        intEventTest.Raise(2);
        voidEventTest.Raise();
    }


    public void TestVoidEventResponse(string _value){
        Debug.Log("Value from voidEvent: " + _value);
        Debug.Assert(_value == "VoidTypeEvent");
    }

    public void TestIntEventResponse(int _value){
        Debug.Log("Value from int-event: " + _value);
        Debug.Assert(_value == 2 || _value == 5);
    }
}
