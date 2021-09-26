using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBar : MonoBehaviour
{

    public SpriteRenderer bar;
    public Sprite[] bars;
    public Sprite[] ammos;

    public GameObject bulletInst;
    public List<GameObject> bullets = new List<GameObject>();




    //Makes a new bar
   public void Reload (int ammo, int type)
    {
        //Load Bar
        if (type != 0 && type != 4) { bar.gameObject.SetActive(true); bar.sprite = bars[type - 1]; }
        else
        { bar.gameObject.SetActive(false); }

        //Remove Bullets
        foreach(GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        bullets.Clear();



        //Initialize bullets
        if (ammo != 0)
        {
            for (int i = 0; i < ammo; i++)
            {
                bullets.Add(Instantiate(bulletInst, bulletInst.transform.parent));
                bullets[i].transform.localPosition = new Vector3(0.2f * (i + 1), 0, -0.1f);
                bullets[i].GetComponent<SpriteRenderer>().sprite = ammos[type -1 ];
                bullets[i].SetActive(true);
                
            }
        }
    }

    //Removes the last bullet;
   public void Shot()
    {
        Destroy(bullets[bullets.Count - 1]);
        bullets.Remove(bullets[bullets.Count - 1]);
    }


}
