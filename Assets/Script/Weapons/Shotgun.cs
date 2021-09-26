using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    private void Awake()
    {
        id = 2;
    }

    int pellets = 12;

    public override void Shoot(Vector2 aim)
    {
        base.Shoot(aim);

        //Shoot multiple pellets in random arcs
        for (int i = 1; i < pellets + 1; i++)
        {
            Vector3 randomDirection = Quaternion.Euler(0, 0, Random.Range(-1.7f * i, 2f * 1.7f)) * aim;
            float range = Random.Range(1.0f - (i / 10.0f), 1);
            float randomRange = Statistics.weapons[2].range * range;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, randomDirection, randomRange, Statistics.mask);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<IDamage>() != null)
            {
                float damage = (Statistics.weapons[2].damage / pellets) - (Vector2.Distance(transform.position, hit.transform.position) * Statistics.weapons[2].falloff);
                if (damage > 0)
                    hit.collider.gameObject.GetComponent<IDamage>().Damage(damage, transform.position, Statistics.weapons[2].knockback, hit.point);
            }
            else
            {

                if (hit)
                {
                    Instantiate(Resources.Load("Objects/ShotChip"), (hit.point + new Vector2(0, 0.5f)), Quaternion.identity);
                    Debug.DrawLine(transform.position, hit.point, Color.red, 1);
                }
                if (!hit)
                {
                    Instantiate(Resources.Load("Objects/ShotChip"), transform.position + (randomDirection * randomRange), Quaternion.identity);
                    Debug.DrawLine(transform.position, transform.position + (randomDirection * randomRange), Color.red, 1);
                }
            }
        } 
    }
}
