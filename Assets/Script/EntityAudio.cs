using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityAudio
{

    // Makes an empty gameobject with a sound and destroys it after while
    public static void MakeSound(string file, Transform trans, float volume = 0.5f, int priority = 150, bool blend = false, bool stick = false)
    {

        GameObject go = new GameObject("" + file + "_Audio");
        go.transform.position = trans.position;
        if (stick) { go.transform.parent = trans; }
        AudioSource sound = go.AddComponent<AudioSource>();
        sound.playOnAwake = false;
        sound.clip = Resources.Load<AudioClip>("Audio/" + file);
        if (blend)sound.spatialBlend = 0.7f;
        sound.volume = volume;
        sound.priority = priority;
        sound.Play();
        go.AddComponent<TimedDestruction>();

    }
}
