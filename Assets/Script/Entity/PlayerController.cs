using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles stuff exclusive to the player, like input, also animations trigerred by input
public class PlayerController : MonoBehaviour, IDamage, IArmedEntity
{

    // Stats
    float maxhp;
    public float hp;
    public float speed;
    public string[] oneLiners;


    // Values
    float cooldown;
    float stun;
    public int killCount;
    public bool behindCover;
    Vector3 moveDirection;
    Vector3 aimDirection;
    public Vector3 offset;
    bool moves;
    IAttack fists;
    IAttack granades;
    public IAttack weapon;
    public int weaponId;
    public float damageSoundCooldown = 0;


    // Elements
    public AmmoBar ammoBar;
    ParticleSystem blood;
    AudioSource audio;
    Animator animator;
    Rigidbody2D rig;
    GameObject arms;
    Animator armsanim;
    SpriteRenderer sarms;
    SpriteRenderer sprite;
    PixelationManager pixelation;

    private void Awake()
    {
        maxhp = hp;
        arms = transform.Find("Arms").gameObject;
        fists = GetComponent<Fists>();
        granades = GetComponent<GranadesManager>();
        blood = transform.Find("Blood").GetComponent<ParticleSystem>();
        armsanim = arms.GetComponent<Animator>();
        sarms = arms.GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        pixelation = Camera.main.gameObject.GetComponent<PixelationManager>();
        GameManager.EnemyKill += KillOneLiner;
        Invoke("Startle", 1f);

    }

    private void Update()
    {

        // Weapons

        // Cooldown
        if (cooldown > 0) { cooldown -= 2.0f * Time.deltaTime; }
        if (stun > 0) { stun -= 6.0f * Time.deltaTime; }

        // Shoot
        if (weapon!=null && cooldown <= 0 && (Input.GetKeyDown(KeyCode.Mouse0) || (Input.GetKey(KeyCode.Mouse0) && weaponId == 3) ))
        {           
            weapon.Attack(aimDirection);
        }

        if (cooldown <= 0 && Input.GetKeyDown(KeyCode.Mouse1))
        {
            armsanim.SetTrigger("Punch");
            cooldown = Statistics.weapons[0].speed;
            fists.Attack(aimDirection);
        }

        if (weapon == null && cooldown <= 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            armsanim.SetTrigger("Punch");
            cooldown = Statistics.weapons[0].speed;
            fists.Attack(aimDirection);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            granades.Attack(aimDirection);
        }

        // Movement
        moveDirection = new Vector3();

        bool tempMoveSensor = moves;
        moves = false;

        // Get movement keys
        if (Input.GetKey(KeyCode.W))
        {
            moves = true;
            moveDirection += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moves = true;
            moveDirection += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moves = true;
            moveDirection += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moves = true;
            moveDirection += Vector3.right;
        }

        if (stun <= 0)
        {

            // Move audio
            if (tempMoveSensor == false  && moves ==true ) { StartCoroutine("WalkSound"); }
            if (tempMoveSensor == true && moves == false) {  StopCoroutine("WalkSound"); }

            // Animation
            animator.SetBool("Walk", moves);

            // Always aiming
            Vector3 mouse = Input.mousePosition;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, transform.position.z)) - new Vector3(0, 0.5f, 0);
            aimDirection = mouseWorld - (transform.position + offset);
            aimDirection.z = 0;
            aimDirection = aimDirection.normalized;


            // Get mouse pos and change animation direction
            if (Mathf.Abs(aimDirection.x) > Mathf.Abs(aimDirection.y))
            {
                animator.SetInteger("Direction", 0);
                armsanim.SetInteger("Direction", 0);
                sprite.flipX = (aimDirection.x < 0);
                sarms.flipX = (aimDirection.x < 0);
                arms.transform.localPosition = new Vector3(0, 0.6f, -0.1f);
            }
            else
            {
                sprite.flipX = false;
                sarms.flipX = false;
                animator.SetInteger("Direction", aimDirection.y > 0 ? 1 : -1);
                armsanim.SetInteger("Direction", aimDirection.y > 0 ? 1 : -1);
                arms.transform.localPosition = new Vector3(0, 0.6f, aimDirection.y > 0 ? 0.01f : -0.01f);
            }


            // Behind cover debug
            //Debug.DrawLine(transform.position, transform.position + (moveDirection * 0.3f), Color.blue, Time.deltaTime);
            //Debug.DrawLine(transform.position + offset, (transform.position + offset) + (aimDirection * 1.4f), Color.gray, Time.deltaTime);
        } 
        else
        {
            // Stun
            animator.SetBool("Walk", false);
            StopCoroutine(WalkSound());
        }
    }

    private void FixedUpdate()
    {
        // Move
        if (stun <= 0) rig.velocity = moveDirection.normalized * speed;// * 20 * Time.deltaTime;
        // Sound cooldown
        if (damageSoundCooldown > 0){
            damageSoundCooldown -= 0.1f;
        }
    }

    IEnumerator WalkSound()
    {
        while (true)
        {
            audio.Play();
            yield return new WaitForSeconds(0.3f);
        }
    }


    void DamageSound()
    {
        if (damageSoundCooldown <= 0) {
            EntityAudio.MakeSound("y", transform, 0.3f);
        damageSoundCooldown = 5;
        } 
    }

    void KillOneLiner()
    {
        killCount++;
        if (killCount % 6 == 0 || killCount == 3)
        {
            EntityAudio.MakeSound(oneLiners[Random.Range(0, oneLiners.Length)], transform, 0.3f, 1, false, true);
        }
    }

    void Startle()
    {
            EntityAudio.MakeSound("okurwa", transform, 0.36f, 1, false, true);
    }


    //IDamage
    public void Damage(float damage, Vector3 position, float knockback, Vector3 hitPoint)
    {
        if (hp > 0)
        {
            DamageSound();
            // Bleed
            blood.transform.position = hitPoint + Statistics.gunOffset;
            blood.Emit(Mathf.RoundToInt(damage * 5));
            // Stun
            if (knockback > stun) stun = knockback;
            // Knockback
            rig.velocity = Vector3.zero;
            rig.AddForce((transform.position - position).normalized *  10 * knockback);
            // Camera effect
            pixelation.intensity += damage;
            // Damage
            hp -= damage;
        }
        if (hp <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        // Animation
        animator.SetTrigger("Die");
        // Event
        GameManager.playerDead = true;
        // Sound
        EntityAudio.MakeSound("pfrt", transform);
        // Destroy
        Destroy(gameObject.GetComponent<Rigidbody2D>(), 1);
        Destroy(gameObject.GetComponent<Collider2D>());
        blood.transform.parent = null;
        Destroy(blood.gameObject, 5);
        foreach (Transform child in transform) { GameObject.Destroy(child.gameObject); }

        Destroy(this);
    }

    //IArmedEntity
    public void Equip(IAttack wp, int id)
    {
        weapon = wp;
        weaponId = id;
        arms.GetComponent<Arms>().Switch(id);
        ammoBar.Reload(Statistics.weapons[id].ammo, id);
    }

    public void Unequip()
    {
        weapon = null;
        weaponId = 0;
        arms.GetComponent<Arms>().Switch(0);
        ammoBar.Reload(0, 0);
    }
    
    public void Attacked()
    {
        //Camera effect
        pixelation.intensity += 1;
        cooldown = Statistics.weapons[weaponId].speed;
        ammoBar.Shot();
        armsanim.SetTrigger("Attack");
    }

}
