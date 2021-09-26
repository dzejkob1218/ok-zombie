using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x;
        float y;

        x = Mathf.Lerp(transform.position.x, player.position.x, 0.1f);
        y = Mathf.Lerp(transform.position.y, player.position.y, 0.1f);
        x = Mathf.Clamp(x, 11.8f, 37);
        y = Mathf.Clamp(y, 7, 23);
        transform.position = new Vector3(x, y, player.position.z - 10); 
    }
}
