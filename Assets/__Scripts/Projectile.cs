using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rigid;
    protected BoundsCheck _bounds;
    protected WeaponType _type;
    public WeaponType type
    {
        get {return _type; }
        set { SetType(value); }
    }

    // Start is called before the first frame update
    protected void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        _bounds = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!_bounds.onScreen)
        {
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType eWeapon)
    {
        _type = eWeapon;
    }
}
