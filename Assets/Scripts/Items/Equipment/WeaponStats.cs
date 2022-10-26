using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SmallTimeRogue.Stats
{
    [System.Serializable]
    public class WeaponStats : ItemStats
    {
        [FoldoutGroup("Weapon Stats")] public int damage = 10;
        [FoldoutGroup("Weapon Stats")] public int armorPenetration = 0;
        [FoldoutGroup("Weapon Stats")] public int critChance = 1;
        [FoldoutGroup("Weapon Stats")] public int critDamage = 0;
        [FoldoutGroup("Weapon Stats")] public float knockBack = 5;
        [FoldoutGroup("Weapon Stats")] public float primaryCooldown = 0.2f; //How long at a base a weapon needs to wait before it can attack again.
        [FoldoutGroup("Weapon Stats")] public float primaryDuration = 0.05f; //Minimum time before weapon can attack again!
        [FoldoutGroup("Weapon Stats")] public float secondaryCooldown = 1f; //Minimum time before weapon can attack again!
        [FoldoutGroup("Weapon Stats")] public float secondaryDuration = 0.5f; //Minimum time before weapon can attack again!

        [FoldoutGroup("Weapon Settings")] public float moveSpeedModifier = 0; //How much to slow / speed up the player when equipped. 0 = no change. 1 = +100%.
        [FoldoutGroup("Weapon Settings")] public bool slowsPlayerWhenAttacking; //How much to slow the player by when attacking.
        [FoldoutGroup("Weapon Settings")] public bool allowsHoldingPrimaryDown, allowsHoldingSecondaryDown; //Whether the player is allowed to auto-attack by just holding down the key.

        [FoldoutGroup("Weapon Effects")] public GameObject primaryEffect, secondaryEffect;
    }
}