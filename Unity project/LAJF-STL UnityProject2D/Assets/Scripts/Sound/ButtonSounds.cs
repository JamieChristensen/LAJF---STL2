using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STL2.Events;

public class ButtonSounds : MonoBehaviour
{
    public VoidEvent choiceMadeSound;
  
    public void OnChoiceMade()
    {
        choiceMadeSound.Raise();
    }

}
