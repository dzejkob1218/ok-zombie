using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    CircleCollider2D col;
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        Invoke("Explode",1.3f);
    }

    private void FixedUpdate()
    {
       
        col.offset = new Vector2(0, Mathf.Lerp(col.offset.y, 0, 0.05f));
    }

    // Update is called once per frame
    void Explode ()
    {
        foreach (Transform child in transform)
        {
            child.parent = null;
            child.gameObject.active = true;

        }
        Destroy(gameObject);
    }
}
