using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int id;
    public Sprite[] sprites;
    int damage;
    private void Start()
    {
        damage = id * 2;
        GetComponent<SpriteRenderer>().sprite = sprites[id - 1];
        Destroy(this, 15);
    }

    // Start is called before the first frame update
   void OnCollisionEnter2D (Collision2D col)
    {
       if(col.gameObject.tag == "Enemy")
        {
            IDamage enemy = col.gameObject.GetComponent<IDamage>();
            enemy.Damage(4, transform.position, 2, (transform.position - new Vector3(0,0.5f,0)));
        }

        if (col.gameObject.tag != "Player")
        {
            gameObject.GetComponent<ParticleSystem>().Emit(30);
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 5);
            Destroy(this);
        }
    }
}
