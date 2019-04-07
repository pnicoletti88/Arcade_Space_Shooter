using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // script is intended to deal with input from the user and work with menuButtonController to load the correct scene, or perform the right operation when a button is pressed.
    [SerializeField] public MenuButtonController menuButtonController;
    public static LoadScene sceneLoader;
    public bool exitScene = false;

    private const string _TWITTER_ADDRESS = "http://twitter.com/intent/tweet"; //link to twitter
    private const string _TWEET_LANGUAGE = "en"; //twitter language

    void Awake()
    {
        if (sceneLoader == null)
        {
            sceneLoader = this; //sets up singleton so that only 1 sceneLoader can be created.
        }
        else
        {
            Debug.LogError("Attempted Creation of Second sceneLoader");
        }
    }


    void Update()
    {
        // If New game is selected, and submit is pressed, launch the new game
        if (Input.GetAxis("Submit") == 1 && menuButtonController.index == 0)
        {
            // Use a coroutine to load the Scene in the background, Asynchronous load is used to prevent hiccups and allow animations to fully run.
            StartCoroutine(LoadYourAsyncScene("Main_Scene"));
            
        }
        // If Return to title screen button game is selected, and submit is pressed, return to the title screen
        else if (Input.GetAxis("Submit") == 1 && menuButtonController.index == 1)
        {
            // Use a coroutine to load the Scene in the background, Asynchronous load is used to prevent hiccups and allow animations to fully run.
            if (SceneManager.GetActiveScene().name != "Title_Screen") {
                StartCoroutine(LoadYourAsyncScene("Title_Screen"));
            }
            /* the if statement is there to ensure that if the "Quit Game" button on the title screen (which also has index 1) is pressed, nothing happens. 
             * This button is included as a Proof-of-Concept, as the application cannot be exited within unity. 
             * The game must be taken to build stages for it to function as an isolated application, and thus require a "quit game" button. */
        }
        else if (Input.GetAxis("Submit") == 1 && menuButtonController.index == 2)
        {
            HandleClickPost(); // calls function to post to twitter
        }
    }

    
    IEnumerator LoadYourAsyncScene(string scene)
    {
        exitScene = true;
        yield return new WaitForSeconds(3.0f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        // Wait until the asynchronous scene fully loads, but alos give minimum 3 second wait time to allow for animations/noises to execute, and for the FadeTextToZeroAlpha coroutine to finish.
        // Scene is loaded asynchronously to minimze issues and noticeable framerate drops with the transition between scenes, given the events that occur at the scene exit, and the events that occur when a the new game begins.
        while (!asyncLoad.isDone)
        {
            
            yield return null;
        }
    }



    //function that handles posting the final score to twitter using URL
    void HandleClickPost()
    {
        string tweet;
        if (PlayerPrefs.HasKey("currScore"))
        {
            tweet = "My Final Score is: " + PlayerPrefs.GetInt("currScore");
        }
        else
        {
            tweet = "My Final Score is: " + 0;
        }
        Application.OpenURL(_TWITTER_ADDRESS +
                    "?text=" + WWW.EscapeURL(tweet) +
                    "&amp;lang=" + WWW.EscapeURL(_TWEET_LANGUAGE));
    }

}
