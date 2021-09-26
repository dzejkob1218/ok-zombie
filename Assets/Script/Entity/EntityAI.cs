using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EntityAI : MonoBehaviour, IDamage, IArmedEntity
{
    // Stats
    protected float maxhp;
    public float hp;
    public float speed;
    public float attackSpeed;
    public float attackDistance;
    public float disarmChance = 0;
    public float lootChance = 0;
    public int ammo;

    // Values
    public float distance;
    public float stun;
    protected Transform target;
    protected Vector3 direction;

    protected bool armed = true;

    // Elements
    protected IAttack attack;
    protected AILerp move;
    protected AIDestinationSetter dest;
    protected Rigidbody2D rig;
    protected Animator anim;
    protected ParticleSystem blood;

    //Behaviour:
    //-Find Enemies (Or Tasks) Close By
    //-Assign Priority
    //-Pick Target (Or Waypoint)
    //-Move To Position
    //-Attack (Or Interact) When Up Close


    void Start()
    {
        attack = GetComponent<IAttack>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        move = GetComponent<AILerp>();
        move.speed = speed;
        dest = GetComponent<AIDestinationSetter>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        dest.target = target;
        blood = transform.Find("Blood").GetComponent<ParticleSystem>();
    }

    void Update()
    {

        if (hp <= 0)
        {
            Die();
        }

        // Cooldown and drag
        if (stun > 0)
        {
            move.canMove = false;
            anim.SetBool("Walk", false);
            stun -= 2f * Time.deltaTime;
        }
        // Move
        else
        {
            move.canMove = true;
            anim.SetBool("Walk", true);
        }
        rig.drag = 1 + stun;


        // Turning
        direction = (target.position - transform.position).normalized;
        transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);


        CheckAttack();

    }

    // IDamage
    public void Damage(float damage, Vector3 position, float knockback, Vector3 hitPoint)
    {
        //Damage
        hp -= damage;

        if (hp > 0)
        {
            //Bleed
            blood.transform.position = hitPoint + Statistics.gunOffset;
            blood.Emit(Mathf.RoundToInt(damage * 3));
            //Stun
            if (knockback > stun) stun = knockback;
            //Knockback
            rig.velocity = Vector3.zero;
            rig.AddForce((transform.position - position).normalized * (damage * 2) * 4 * knockback);
            // Chance to disarm
            if (armed && Random.value < disarmChance)
            {
                Unequip();
            }
        }
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //Death animation
        anim.SetTrigger("Die");
        //Increase spite
        Spawner.spite += 0.2f / (Spawner.spite / 3);
        //Event
        Camera.main.gameObject.GetComponent<GameManager>().OnEnemyKilled();
        // Trigger sound
        EntityAudio.MakeSound("pfrt", transform, 0.3f); 
        // Destroy the object
        Destroy(gameObject.GetComponent<Rigidbody2D>(), 1);
        Destroy(gameObject.GetComponent<Collider2D>());
        blood.transform.parent = null;
        Destroy(blood.gameObject, 5);
        foreach (Transform child in transform) { GameObject.Destroy(child.gameObject); }
        Destroy(gameObject.GetComponent<Pathfinding.SimpleSmoothModifier>()); Destroy(dest);
        Destroy(move); Destroy(gameObject.GetComponent<Pathfinding.Seeker>());
        Destroy(gameObject, 15); // TODO: Replace with object pooling
        if (Random.value < lootChance) Instantiate(Resources.Load("Objects/Loot"), transform.position, Quaternion.identity);
        Destroy(this);
    }

    // IArmedEntity
    public void Attacked()
    {
    }

    public void Equip(IAttack wp, int id)
    {

    }

    public void Unequip()
    {
        if (armed){
            armed = false;
            Destroy(transform.Find("Gun").gameObject);
            attack = gameObject.AddComponent<ZombieJab>();
            attackDistance = 0.6f;
            attackSpeed = 3;
            speed += 1;
        }
    }

    public virtual void CheckAttack()
    {
        if (attack != null && stun <= 0 && (Vector2.Distance(target.position, transform.position) < attackDistance))
        {
            // Check if the target is in view
            Vector2 aimDir = target.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDir, attackDistance * 1.2f, Statistics.mask);
            stun = attackSpeed;

            if (hit && hit.transform.gameObject == target.gameObject)
            {
                attack.Attack(direction);
            }
        }
    }
}
