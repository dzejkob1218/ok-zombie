using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGrenade : MonoBehaviour
{
    GranadesManager entity;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            entity = col.gameObject.GetComponent<GranadesManager>();
            if (entity.ammo < 3)
            {
                entity.ammo++;
                entity.UpdateSprites();
                Destroy(gameObject);
            }

        }
    }
}