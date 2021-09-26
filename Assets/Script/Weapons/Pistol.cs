using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    private void Awake()
    {
        id = 1;
    }

    public override void Shoot(Vector2 aim)
    {
        base.Shoot(aim);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, aim, Statistics.weapons[1].range, Statistics.mask);
        if (hit.collider != null && hit.collider.gameObject.GetComponent<IDamage>() != null)
        {
            float damage = Statistics.weapons[1].damage - ((Vector2.Distance(transform.position, hit.transform.position) * Statistics.weapons[1].falloff));
            if (damage > 0)
                hit.collider.gameObject.GetComponent<IDamage>().Damage(damage, transform.position, Statistics.weapons[1].knockback, hit.point);
        }
        else
        {

            // Draw chips
            if (hit)
            {
                Instantiate(Resources.Load("Objects/ShotChip"), (hit.point + new Vector2(0, 0.5f)), Quaternion.identity);
                Debug.DrawLine(transform.position, hit.point, Color.red, 1);
            }
            if (!hit)
            {
                Instantiate(Resources.Load("Objects/ShotChip"), transform.position + (new Vector3(aim.x, aim.y, 0) * Random.Range(10.0f, 15.0f)), Quaternion.identity);
                Debug.DrawLine(transform.position, transform.position + (new Vector3(aim.x, aim.y, 0) * Statistics.weapons[1].range), Color.red, 0.6f);
            }
        }

    }
}
