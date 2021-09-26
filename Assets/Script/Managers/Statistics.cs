using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Statistics : MonoBehaviour
{
    public static LayerMask mask = ~((1 << 9) | (1 << 2) );
    public static Vector3 gunOffset = new Vector3(0, 0.5f, 0);
    public static List<WeaponStats> weapons = new List<WeaponStats>();

    private void Awake()
    {
                             //name,    damage, speed, ammo, knockback, falloff, range
        weapons.Add(new WeaponStats("Fists",  4, 0.4f,  0,    4,         0,      0.9f));
        weapons.Add(new WeaponStats("Pistol", 5, 0.2f,  12,   0.8f,      0.2f,    24));
        weapons.Add(new WeaponStats("Shotgun",13,0.4f,  9,    1.2f,      0.1f,    20));
        weapons.Add(new WeaponStats("Rifle",  5, 0.3f,  18,   0.5f,      0.3f,    28));
        weapons.Add(new WeaponStats("Sword",  4, 0.5f,  0,    0.4f,      0,       1.2f));

    } 

    public static WeaponStats SwitchWeapon(int id)
    {
        Debug.Log("Returning Stats");
        return weapons[id];

    }

}
