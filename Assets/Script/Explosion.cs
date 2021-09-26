using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage;
    private void Start()
    {
        Destroy(this, 0.1f);
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<IDamage>() != null)
        {
            IDamage hit = col.gameObject.GetComponent<IDamage>();
            if (damage > 0)
                hit.Damage(damage, transform.position, 2, col.gameObject.transform.position);
        }

    }
}
