using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fists : MonoBehaviour, IAttack
{

    public void Attack(Vector2 aim)
    {
        EntityAudio.MakeSound("jeb",transform, 0.2f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aim, Statistics.weapons[0].range, Statistics.mask);
        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            hit.collider.gameObject.GetComponent<IDamage>().Damage(Statistics.weapons[0].damage, transform.position, Statistics.weapons[0].knockback, hit.point);
        }
    }
}
