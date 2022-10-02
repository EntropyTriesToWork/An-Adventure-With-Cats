using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SmallTimeRogue.Items
{
    [System.Serializable]
    public class WeaponStats : ItemStats
    {
        [BoxGroup("Weapon")] public int damage = 10;
        [BoxGroup("Weapon")] public int armorPenetration = 0;
        [BoxGroup("Weapon")] public int critChance = 1;
        [BoxGroup("Weapon")] public int critDamage = 150;
        [BoxGroup("Weapon")] public float knockBack = 5;

        [BoxGroup("Weapon")] public float attackDelay = 0.2f; //How long at a base a weapon needs to wait before it can attack again.
        [BoxGroup("Weapon")] public float attackDuration = 0.02f; //Minimum time before weapon can attack again!
        [BoxGroup("Weapon")] public float moveSpeedModifier = 0; //How much to slow / speed up the player when equipped. 0 = no change. 1 = +100%.
        [BoxGroup("Weapon")] public bool slowsPlayerWhenAttacking; //How much to slow the player by when attacking.

        [BoxGroup("Weapon")] public GameObject attackEffect;
    }
}