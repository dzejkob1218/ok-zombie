using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(0.0625f, 0.0625f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
