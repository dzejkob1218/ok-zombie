using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelationManager : MonoBehaviour
{

    public float intensity;
    public Pixelate cameraFilter;

    // Start is called before the first frame update
    void Start()
    {
        cameraFilter = GetComponent<Pixelate>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if (intensity > 10) {
            intensity = 10;
        }

        if (intensity > 0){
            int round_intensity = (int) Mathf.Round(intensity);
            cameraFilter.ChangePixelSize(round_intensity);
            intensity -= 0.1f;
        }



    }
}
