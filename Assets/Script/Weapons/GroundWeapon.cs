using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundWeapon : MonoBehaviour
{
    public int id;
    public Sprite[] sprites;
    Gun wp;

    private void Awake()
    {
        id = Random.Range(1, 4);
        GetComponent<SpriteRenderer>().sprite = sprites[id - 1];
    }

    private void OnTriggerStay2D(Collider2D col)
    {
         if (col.gameObject.tag == "Player")
        {

            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            if (player.weapon != null) return;

            switch (id)
            {
                case 1: wp = player.gameObject.AddComponent<Pistol>(); break;
                case 2: wp = player.gameObject.AddComponent<Shotgun>(); break;
                case 3: wp = player.gameObject.AddComponent<Rifle>(); break;
            }

            //wp.id = id;
            //wp.entity = player.GetComponent<PlayerController>();
            Destroy(gameObject);

        }
    }
}
