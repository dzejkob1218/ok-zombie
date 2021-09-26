using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitPair
{
    public GameObject obj; // The object hit
    public int lay; 

    public HitPair(GameObject new_obj, int new_lay)
    {
        obj = new_obj;
        lay = new_lay;
    }
}
public class Rifle : Gun
{
    private void Awake()
    {
        id = 3;
    }

    public override void Shoot(Vector2 aim)
    {
        base.Shoot(aim);

        Vector3 randomDirection = Quaternion.Euler(0, 0, Random.Range(-2f, 2f)) * aim;
        List<HitPair> hitlist = new List<HitPair>();
        float pierce = 3;


        // Pierce three enemies before stopping
        while (pierce>0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, randomDirection, Statistics.weapons[3].range, Statistics.mask);
            if (!hit)
            {
                //END OF REACH
                Instantiate(Resources.Load("Objects/ShotChip"), transform.position + (randomDirection * Random.Range(10.0f, 15.0f)), Quaternion.identity);
                break;
            }
            else if (hit)
            {
              
                GameObject target = hit.collider.gameObject;
                //Decide wether to shoot again or/and damage
                 //if (target.layer == 11) { pierce -= Random.Range(1.3f, 1.7f); }
                //Damage
                if (target.GetComponent<IDamage>() != null)
                {
                    float dmg = (Statistics.weapons[3].damage * 0.33f * pierce) - ((Vector2.Distance(transform.position, hit.point) * Statistics.weapons[3].falloff));
                    target.GetComponent<IDamage>().Damage(dmg, transform.position, Statistics.weapons[id].knockback, hit.point);
                } else
                {
                    Instantiate(Resources.Load("Objects/ShotChip"), (hit.point + new Vector2(0,0.5f)), Quaternion.identity);
                }

                //Make target unhitable for next time
                hitlist.Add(new HitPair(target, target.layer));
                target.layer = 2; // Ignore raycast on next iteration
                pierce--;
            } else
            {
                break;
            }
        }

        //REVERT LAYERS
        foreach(HitPair revert in hitlist)
        {
            revert.obj.layer = revert.lay;

        }    
    }
}
