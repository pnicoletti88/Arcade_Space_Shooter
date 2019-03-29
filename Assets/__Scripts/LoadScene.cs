using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] public MenuButtonController menuButtonController;
    private AssetBundle myLoadedAssetBundle;

    void Start()
    {
        //LoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/_Scenes");
    }

    void Update()
    {
        // If New game is selected, and submit is pressed, launch the new game
        if (Input.GetAxis("Submit") == 1 && menuButtonController.index == 0)
        {
            // Use a coroutine to load the Scene in the background, Asynchronous load is used to prevent hiccups and allow animations to fully run.
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    
    IEnumerator LoadYourAsyncScene()
    {
        yield return new WaitForSeconds(3);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main_Scene");
        //// Wait until the asynchronous scene fully loads, but alos give minimum 3 second wait time to allow for animations.
        
        while (!asyncLoad.isDone)
        {
            
            yield return null;
        }
    }
    
}
