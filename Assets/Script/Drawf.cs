using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Drawf
{
    public static Vector3 layerShift(Vector3 pos)
    {
        return new Vector3(pos.x, pos.y, pos.y / 10);
    }
}
