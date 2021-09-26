using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IAttack
{
    public IArmedEntity entity;
    public int id;
    public int ammo;
    protected Animator anim;
    // This funtion is same in all weapons
    public virtual void Start()
    {
        ammo = Statistics.weapons[id].ammo;
        entity = GetComponent<IArmedEntity>();
        if (entity != null) entity.Equip(this, id);
    }
    // This funtion is same in all guns
    public virtual void Attack(Vector2 aim)
    {
        //Deduct ammo
        if (ammo > 0)
        {        
            ammo--;
            Shoot(aim);
            if (entity != null )entity.Attacked();
       }
        else { Throw(); }
    }

    // Since every weapon shoots differently it's separated here to be completely overriden in most cases
    public virtual void Shoot(Vector2 aim)
    {
        EntityAudio.MakeSound("pif", transform, 0.1f, 150, true, false);
    }

    void Throw()
    {
        entity.Unequip();
        Destroy(this);
    }

}
