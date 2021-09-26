using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IDamage
{
    void Damage(float damage, Vector3 position, float knockback, Vector3 hitPoint);
    void Die();
}
