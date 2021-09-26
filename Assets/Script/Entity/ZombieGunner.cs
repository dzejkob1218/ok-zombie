using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGunner : EntityAI
{

    public SpriteRenderer Fire;

    // Overrides deafault EntityAI attack to use a gun
    public override void CheckAttack()
    {
        if (!armed){
            base.CheckAttack();
        }
        // Check if zombie can attack
        else if (attack != null && ammo > 0 && stun <= 0 && (Vector2.Distance(target.position, transform.position) < (attackDistance * 0.9f)))
        {
            // Check if line of fire is clear
            Vector2 aimDir = target.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDir, attackDistance * 1.2f, Statistics.mask);

            if (hit && hit.transform.gameObject == target.gameObject)
            {
                Fire.enabled = true;
                Invoke("HideFire", 0.1f);
                stun = attackSpeed;
                attack.Attack(direction);
                ammo--;
                if (ammo <= 0){
                    Invoke("Unequip", 1f);
                }
            }
        }
        

    }

    void HideFire() {
      Fire.enabled = false;
    }
}
