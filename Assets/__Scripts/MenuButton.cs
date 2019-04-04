using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] public MenuButtonController menuButtonController;
    [SerializeField] public Animator animator;
    [SerializeField] public AnimatorFunctions animatorFunctions;
    [SerializeField] public int thisIndex;
    public LoadScene sceneLoader;

    private int _waitFor = 9;
    private int _framesWaited = 0;
    private IEnumerator _coroutine;
    private FadeText _fadeText;
    
    void Start()
    {
        animator.enabled = true; // enables the animator when object is created
        _fadeText = GetComponent<FadeText>(); // creates a reference to the script that fades text to zero alpha.
    }

    void Update()
    {
        if (_framesWaited > _waitFor) // this waits 10 frames before doing anything - when loading in the scene again, there were slight jitters. This minimizes this.
        {
            if (menuButtonController.index == thisIndex) // if the controller tells the script that it is selected, then run the animation (based on the boolean paramater)
            {
                animator.SetBool("Selected", true);
                if (Input.GetAxis("Submit") == 1) // run the press animation if the key is pressed.
                {
                    animator.SetBool("Press", true);
                }
                else if (animator.GetBool("Press")) // runs the next animation if the Pressed boolean is true, and prevents it from looping by immediately setting it to false.
                {
                    animator.SetBool("Press", false);
                }
            }
            else
            { // if the menubuttonController's index is not this button, then selected is false.
                animator.SetBool("Selected", false);
            }
            if (sceneLoader.exitScene)
            {
                // starts the coroutine before the new scene is loaded
                StartCoroutine(waitAnimation());
            }
        }
        else
        {
            _framesWaited++;
        }

    }
    public IEnumerator waitAnimation()
    {
        // coroutine waits 2 seconds for animations to finish, then disables the animator. This is neccessary (and easiest to do in this scirpt), because if the animator is enabled, it will set the colour of each object to it's current state in the animator (likely deselected or selected), and prevents the FadeTextToZeroAlpha coroutine from working properly.
        yield return new WaitForSeconds(2.0f);
        animator.enabled = false;
        _fadeText.exit = true; // lets the fadeText item know that it can begin the coroutine of fading way the UI element.
    }
}
