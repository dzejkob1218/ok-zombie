using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateSprite : MonoBehaviour
{

    public float speed;
    public List<Sprite> sprites = new List<Sprite>();
    SpriteRenderer rend;
    int i;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>(); 
        i = 0;
        StartCoroutine("Animate");
    }

    // Update is called once per frame
    IEnumerator Animate()
    {
        yield return new WaitForSeconds(Random.Range (0.0f,speed));
        while (true)
        {
            rend.sprite = sprites[i];
            i++;
            if (i == sprites.Count) i = 0;
            yield return new WaitForSeconds(speed);

        }
        
    }
}
