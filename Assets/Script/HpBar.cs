using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    public SpriteRenderer[] bar;
    PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 9; i >= 0; i --)

        {
            bar[i].enabled = ((Mathf.RoundToInt(player.hp/10) - 1) >= i);
        }
    }
}
