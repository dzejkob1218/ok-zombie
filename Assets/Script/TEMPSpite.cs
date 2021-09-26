using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPSpite : MonoBehaviour
{
    PlayerController player;
    GranadesManager grenades;
    TextMesh text;
    void Start()
    {
        text = GetComponent<TextMesh>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        grenades = GameObject.FindGameObjectWithTag("Player").GetComponent<GranadesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + grenades.ammo + ", " + player.killCount + " , " + Mathf.RoundToInt(Spawner.spite);
    }
}
