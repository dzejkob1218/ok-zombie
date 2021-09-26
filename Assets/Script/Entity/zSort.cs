using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zSort : MonoBehaviour
{
    // Prevents objects from obstructing one another by moving along z axis according to y
    private void Awake()
    {
       if (GetComponent<Rigidbody2D>() == null)
        {
            transform.position = Drawf.layerShift(transform.position);
            Destroy(this);
        }
    }

    private void Update()
    {
        transform.position = Drawf.layerShift(transform.position);
    }
}
