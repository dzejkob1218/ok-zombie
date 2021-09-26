using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public List<GameObject> loot = new List<GameObject>();
    void Awake()
    {
       int rand = Random.Range(-2, 3);
        rand = Mathf.Clamp(rand, 0, 2);
       Instantiate(loot[rand], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    
}
