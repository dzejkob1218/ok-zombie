using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
  PlayerController entity;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Entered");

            entity = col.gameObject.GetComponent<PlayerController>();
            if (entity.hp < 100)
            {
                entity.hp += 25;
                Destroy(gameObject);
            }

        }
    }
}