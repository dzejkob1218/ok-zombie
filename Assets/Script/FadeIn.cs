using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    SpriteRenderer sprite;
    float vis = 0.0f;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        vis += 0.01f;
        sprite.color = Color.Lerp(Color.clear, Color.white, vis);
    }
    
}
