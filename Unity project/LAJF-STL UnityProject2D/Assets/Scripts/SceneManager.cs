using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Responsible for sceneloading, both additive and non-additive.
//"ALoad" refers to async loading something in. Remember to only load things in async once.
public class SceneManager : MonoBehaviour
{
    public int currentEnvironmentIndex{ get; private set; } = -1; //-1 indicating none.

    public void LoadMainMenu(){
        throw new NotImplementedException();
    }

    public void LoadCredits(){
        throw new NotImplementedException();
    }


    //Loads in new environments
    public void ALoadEnvironment(int sceneIndex){
        //Unload current environment (if one is present)
        //Load in new one
        //Remove loading-screen (transition)
        throw new NotImplementedException();
    }

}
