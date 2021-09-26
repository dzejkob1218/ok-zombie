using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArmedEntity
{
    void Attacked();
    void Equip(IAttack wp, int id);
    void Unequip();
}
