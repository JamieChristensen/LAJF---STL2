using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using STL2.Events;

//Responsible for sceneloading, both additive and non-additive.
//"ALoad" refers to async loading something in. Remember to only load things in async once.
public class CustomSceneManager : MonoBehaviour
{
    [SerializeField]
    private int currentEnvironmentIndex = -1;
    [SerializeField]
    public BoolVariable isLoadingScene;
    //private bool isLoadingScene = false;
    private float loadProgress = 0;

    public IntEvent loadProgressInt;

    public void Start()
    {
        GameObject.DontDestroyOnLoad(this);
        isLoadingScene.setBool(false);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RequestEnvironmentChange(currentEnvironmentIndex == 1 ? 2 : 1);
        }

        if (Input.GetKeyDown(KeyCode.N)){
            StartCoroutine(AUnloadEnvironment(currentEnvironmentIndex));
        }
    }

    public void LoadMainMenu()
    {
        throw new NotImplementedException();
    }
    public void LoadCredits()
    {
        throw new NotImplementedException();
    }

    public void RequestSceneChange(int _sceneIndex){
        //In case there is logic needed to prevent scenechanges in certain situations:
        if (_sceneIndex > SceneManager.sceneCountInBuildSettings-1 && _sceneIndex < 0){
            Debug.Log("Scene request denied, index out of range");
            return;
        }
        
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1);
    }

    public void UpdateLoadProgress(float _loadProgress){
        loadProgressInt.Raise((int)(_loadProgress*100));
    }

    public void RequestEnvironmentChange(int environmentIndex)
    {
        //TODO: Check whether or not we can change environment:
        if (isLoadingScene.myBool)
        {
            return;
        }

        if (environmentIndex == -1)
        {
            isLoadingScene.setBool(true);
            StartCoroutine(ALoadEnvironment(environmentIndex));
            return;
        }

        isLoadingScene.setBool(true);
        StartCoroutine(ChangeEnvironment(currentEnvironmentIndex, environmentIndex));
    }


    #region AdditiveAsync
    //ChangeEnvironment combines two other coroutines, to ensure that before loading the next environment, the previous one is always unloaded first.
    IEnumerator ChangeEnvironment(int currentEnvironment, int newEnvironment)
    {
        if (currentEnvironment < 1)
        {
            StartCoroutine(ALoadEnvironment(newEnvironment));
            yield break;
        }
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentEnvironment, UnloadSceneOptions.None);
        asyncUnload.completed += (AsyncOperation) =>
        {
            StartCoroutine(ALoadEnvironment(newEnvironment));
        };

        while (!asyncUnload.isDone)
        {
            Debug.Log("Unloading progress: " + asyncUnload.progress);
            yield return null;
        }

        yield return null;
    }

    //Loads in new environments
    IEnumerator ALoadEnvironment(int sceneIndex)
    {
        //Unload current environment (if one is present)
        //Load in new one
        //Remove loading-screen (transition)
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        
        asyncLoad.completed += (AsyncOperation) =>
        {
            currentEnvironmentIndex = sceneIndex;
            isLoadingScene.setBool(false);
            loadProgress = 0;
            UpdateLoadProgress(1);
            Debug.Log("Checking how many times this is called");
        };


        while (!asyncLoad.isDone)
        {
            UpdateLoadProgress(asyncLoad.progress);
            Debug.Log("Loading progress: " + asyncLoad.progress);
            loadProgress = asyncLoad.progress;
            yield return null;
        }
    }

    IEnumerator AUnloadEnvironment(int sceneIndex)
    {
        if (sceneIndex == -1)
        {
            yield break;
        }

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneIndex, UnloadSceneOptions.None);
        asyncUnload.completed += (AsyncOperation) => {
            currentEnvironmentIndex = -1;
        };

        while (!asyncUnload.isDone)
        {
            Debug.Log("Unloading progress: " + asyncUnload.progress);
            loadProgress = asyncUnload.progress;
            yield return null;
        }
    }
    #endregion AdditiveAsync
}
