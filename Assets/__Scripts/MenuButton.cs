using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField]  public MenuButtonController menuButtonController;
    [SerializeField]  public Animator animator;
    [SerializeField]  public AnimatorFunctions animatorFunctions;
    [SerializeField]  public int thisIndex;


    void Update()
    {
        if(menuButtonController.index == thisIndex) // if the controller tells the script that it is selected, then run the animation (based on the boolean paramater)
        {
            animator.SetBool("Selected", true);
            if(Input.GetAxis("Submit") == 1) // run the press animation if the key is pressed.
            {
                animator.SetBool("Press", true);
            } else if (animator.GetBool("Press")) // runs the next animation if the Pressed boolean is true, and prevents it from looping by immediately setting it to false.
            {
                animator.SetBool("Press", false);
            }
        } else
        {
            animator.SetBool("Selected", false);
        }
    }
}
