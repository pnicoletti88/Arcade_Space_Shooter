using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp 
{
    private Color _colour;
    private WeaponType _drop;
    private string _text;
    private float _duration;
    private string _tag;

    public PickUp(Color c,WeaponType w, string t, float d, string tag)
    {
        this._colour = c;
        this._drop = w;
        this._text = t;
        this._duration = d;
        this._tag = tag;
    }
    public Color color
    {
        get
        {
            return _colour;
        }
    }
    public WeaponType drop
    {
        get
        {
            return _drop;
        }
    }
    public string text
    {
        get
        {
            return _text;
        }
    }
    public float duration
    {
        get
        {
            return _duration;
        }
    }   
    public string tag
    {
        get
        {
            return _tag;
        }
    }
}
