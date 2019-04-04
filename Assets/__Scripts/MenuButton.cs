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
        animator.enabled = true;
        _fadeText = GetComponent<FadeText>();
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
        yield return new WaitForSeconds(2.5f);
        animator.enabled = false;
        _fadeText.exit = true;
    }
}
