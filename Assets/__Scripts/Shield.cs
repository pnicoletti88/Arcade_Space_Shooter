using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    private Material _mat;

    // Start is called before the first frame update
    void Start()
    {
        //get shield material
        _mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //set current level to the shield level of the hero
        int currLevel = Mathf.FloorToInt(Hero_Script.heroScriptReference.shieldLevel);
        if (levelShown != currLevel)
        {
            //ensure that the level of shield shown is equal to the current shield level
            levelShown = currLevel;
            //displays the proper shield that corresponds to the shield level (there are 5 levels 0-4) so 0.2f multiplied by the level sets the correct one
            _mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }

}

