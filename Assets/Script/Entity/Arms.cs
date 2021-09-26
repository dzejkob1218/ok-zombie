using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : MonoBehaviour
{ 
    public Animator anim;
    public AnimatorOverrideController animOverride;
    public int weapo;

    // A class for different animations of a weapon
    [System.Serializable]
    public class WeaponAnimation
    {
        public string name;// String just for clarity of labeling
        public AnimationClip HoldRight;
        public AnimationClip HoldUp;
        public AnimationClip HoldDown;
        public AnimationClip HitRight;
        public AnimationClip HitUp;
        public AnimationClip HitDown;

    }

    public WeaponAnimation[] animations;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        animOverride = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animOverride;

    }

    // Update is called once per frame
    public void Switch(int weapon)
    {
        weapo = weapon;
           animOverride["Arms_HoldRight"] = animations[weapon].HoldRight;
           animOverride["Arms_HoldUp"] = animations[weapon].HoldUp;
           animOverride["Arms_HoldDown"] = animations[weapon].HoldDown;
           animOverride["Pistol_HitRight"] = animations[weapon].HitRight;
           animOverride["Pistol_HitUp"] = animations[weapon].HitUp;
           animOverride["Pistol_HitDown"] = animations[weapon].HitDown;
        
    }
}
