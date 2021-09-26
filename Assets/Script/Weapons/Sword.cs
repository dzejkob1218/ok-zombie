using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword
{ /*
    public override void Update()
    {
        //Throw
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Throw();
        }
        //Attack
        if (entity.cooldown <= 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    public override void Shoot()
    {
        //MAKE LIST OF ALL HIT TARGETS' HEALTH SCRIPTS
        List<Health> hitlist = new List<Health>();

        //SHOOT THREE RAYS AS THE HIT ARC
        for (int i = -1; i < 2; i++)
        {
            Vector3 dir = Quaternion.Euler(0, 0, 35 * i) * entity.aimDirection; //GET AIM DIRECTION
            float range = i == 0 ? stats.range : (stats.range - 0.15f); //MAKE SIDE RAYS SHORTER
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, range); 
            Debug.DrawLine(transform.position, transform.position + (dir * range), Color.red, 0.8f); //EDITOR RAY
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Enemy")
                {
                    Health h = hit.transform.gameObject.GetComponent<Health>();
                    if (!hitlist.Contains(h))
                        hitlist.Add(h);
                }
            }
        }
        foreach (Health enemy in hitlist)
        {
            enemy.Damage(stats.damage, transform.position, stats.knockback);
        }
    }*/
}
