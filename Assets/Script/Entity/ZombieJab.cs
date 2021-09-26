using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieJab : MonoBehaviour, IAttack
{ 
    public void Attack(Vector2 aim)
    {
        //EntityAudio.MakeSound("jeb", transform);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aim,1, Statistics.mask);
        if (hit.collider != null && hit.collider.tag == "Player")
        {
            hit.collider.gameObject.GetComponent<IDamage>().Damage(10, transform.position, 5, hit.point);
        }
    }
}
