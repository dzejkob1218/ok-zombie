using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats : System.Object
{
     public string name;//name of the folder and animator to use
     public float damage;
     public float speed; //How long to wait for next shot
     public int ammo; //Ammo that gun will have when picked up, 0 if melee
     public float knockback;
     public float falloff;//Decides how fast weapon will loose damage with distance, in damages lost per distance
     public float range; // Lenght of raycast



    public WeaponStats(string c_name, float c_damage, float c_speed, int c_ammo, float c_knockback, float c_falloff, float c_range)
    {
       name = c_name;
       falloff = c_falloff;
       speed = c_speed;
       damage = c_damage;
       ammo = c_ammo;
       knockback = c_knockback;
        range = c_range;

    }
}