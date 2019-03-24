using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//can be seen and used by any class

//this is an enum of the differen types of weapons
public enum WeaponType
{
    none,
    single,
    triple,
    homing
}

[System.Serializable]//this allows the system to serialize this class (convert it to a byte array and pass it easily)
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public GameObject projectilePreFab;
    public float damage;
    public float speed;
    public float delayBetweenShots;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;
    [Header("Set Dynamically")]
    [SerializeField]//this forces unity to serialize private fields (can still be set in inspector but field is private)

    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def; //holds the definition of the current weapon
    public GameObject collar;
    public float lastShotTime; // Time last shot was fired 

    private Renderer collarRend; //render of the weapon - will allow for colour switching later on

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>(); //this will be used for colour changing to gun (phase 3)

        SetType(_type); //calls the set type function to initialize the nessesary parameters

        if (PROJECTILE_ANCHOR == null) //creates the parent for the projectiles
        {
            GameObject go = new GameObject("_ProjectileAnchor"); //making new game object
            PROJECTILE_ANCHOR = go.transform;
        }

        GameObject rootGo = transform.root.gameObject;
        if (rootGo.GetComponent<Hero_Script>() != null)
        {
            rootGo.GetComponent<Hero_Script>().fireWeaponsDelegate += Fire; //assigning fire the the function delegate
        }
    }

    void Update()
    {
        //Switch weapon when the key 'c' is clicked
        if(Input.GetKeyDown("c"))
        {
            if(type == WeaponType.single) { type = WeaponType.triple; }
            else if (type == WeaponType.triple) { type = WeaponType.single; }
        }
        
    }

    //property for the weapon type
    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false); //hides the gun if the weapon is none
        }
        else
        {
            this.gameObject.SetActive(true); //show gun if there is a weapon enabled
        }

        def = Main.GetWeaponDefinition(_type);
        lastShotTime = 0; //this means that weapon will be ready to fire right when it is switched to
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy) return; //if there is not weapon then cannot fire

        if (Time.time - lastShotTime < def.delayBetweenShots) //if the last shot was too recent the gun will not fire
        {
            return;
        }

        Projectile p;
        Vector3 vel = Vector3.up * def.speed; //creating a vector3 for the velocity
        if (transform.up.y < 0) //makes sure that the bullet is going to fire "up"
        {
            vel.y = -vel.y;
        }

        switch (type)
        {
            case WeaponType.single: //creates the projectile and fires it forward
                p = MakeProjectile();
                p.rigidBodyProjectile.velocity = vel;
                break;

            case WeaponType.triple: //creates the three projectiles and angles them correctly
                p = MakeProjectile();
                p.rigidBodyProjectile.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigidBodyProjectile.velocity = p.transform.rotation * vel; //changes the velocity vector to rotate projectile
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigidBodyProjectile.velocity = p.transform.rotation * vel;
                break;

            //Please ignore this case - homing missle cannot be switched to in game - will be seen in phase 3!
            case WeaponType.homing:
                p = MakeProjectile();
                p.rigidBodyProjectile.velocity = vel;
                break;
        }
    }
    
    //this function creates a projectiles - returns a reference to script attached to the projectile
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate(def.projectilePreFab);
        if (transform.parent.gameObject.tag == "Hero") //sets up the porjectile as friendly
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else //this section is not currently used but include for future functionality
        {
            go.tag = "ProjectileEnemy"; //makes the projectile from an enemy
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;//puts the projectile the end of the gun
        go.transform.SetParent(PROJECTILE_ANCHOR, true); //giving the projectiles a parent - makes heiarchy much neater
        Projectile p = go.GetComponent<Projectile>();
        p.type = type; //sets the type of projectile - as projectile is a generic class for all projectiles
        lastShotTime = Time.time; //updates the time of the last shot as through making projectile there has just been a shot
        return (p);
    }

    

}
