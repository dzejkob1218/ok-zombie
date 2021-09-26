using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadesManager : MonoBehaviour, IAttack
{
    public int ammo;
    public GameObject grenade;
    
    public SpriteRenderer[] sprites;

    // This funtion is same in all weapons
    public void Attack(Vector2 aim)
    {
        if (ammo > 0)
        {
            ammo--;
            UpdateSprites();
            GameObject proj = Instantiate(grenade, transform.position + new Vector3(0, 0.5f, 0) + (new Vector3(aim.x, aim.y, 0) * 0.2f), Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().AddForce(80 * aim);
        }
    }

    public void UpdateSprites(){
        for (int i = 0; i < 3; i++){
            sprites[i].enabled = ammo > i ? true : false;
        }
    }

}