using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] public MenuButtonController menuButtonController;
    public static LoadScene sceneLoader;
    public bool exitScene = false;


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
        yield return new WaitForSeconds(4.5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        //// Wait until the asynchronous scene fully loads, but alos give minimum 3 second wait time to allow for animations/noises to execute
        
        while (!asyncLoad.isDone)
        {
            
            yield return null;
        }
    }

    private const string _TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string _TWEET_LANGUAGE = "en";

    //function that handles posting the final score to twitter
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
