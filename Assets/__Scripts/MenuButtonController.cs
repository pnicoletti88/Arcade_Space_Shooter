using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    public int index;
    [SerializeField] public int maxIndex;
    [SerializeField] private bool keyDown;

    void Update()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown) // this logic, in addition with the logic at line 42, ensures that if the user "holds" the key down, the menu will only respond once. They must release the key for one frame, have the condtion return to false, and then press a key again - this prevents the user from cycling through the options too quickly.
            {
                if(Input.GetAxis("Vertical") < 0)
                {
                    if(index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0; // this resets the position if you "loop" over, and hit the up arrow when on the top item.
                    }
                } else if (Input.GetAxis("Vertical") > 0)
                {
                    if(index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex; // similarly, this enables the "looping" ability
                    }
                }
                keyDown = true;
            }
        } else
        {
            keyDown = false;
        }
    }
}
