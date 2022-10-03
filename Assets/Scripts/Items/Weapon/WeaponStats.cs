using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SmallTimeRogue.Items
{
    [System.Serializable]
    public class WeaponStats : ItemStats
    {
        [FoldoutGroup("Weapon Stats")] public int damage = 10;
        [FoldoutGroup("Weapon Stats")] public int armorPenetration = 0;
        [FoldoutGroup("Weapon Stats")] public int critChance = 1;
        [FoldoutGroup("Weapon Stats")] public int critDamage = 150;
        [FoldoutGroup("Weapon Stats")] public float knockBack = 5;
        [FoldoutGroup("Weapon Stats")] public float attackCooldown = 0.2f; //How long at a base a weapon needs to wait before it can attack again.
        [FoldoutGroup("Weapon Stats")] public float attackDuration = 0.05f; //Minimum time before weapon can attack again!

        [FoldoutGroup("Weapon Settings")] public float moveSpeedModifier = 0; //How much to slow / speed up the player when equipped. 0 = no change. 1 = +100%.
        [FoldoutGroup("Weapon Settings")] public bool slowsPlayerWhenAttacking; //How much to slow the player by when attacking.
        [FoldoutGroup("Weapon Settings")] public bool allowsHoldingPrimaryDown, allowsHoldingSecondaryDown; //Whether the player is allowed to auto-attack by just holding down the key.
        [FoldoutGroup("Weapon Settings")] public float freezeFrameDuration = 0.1f, freezeFrameScale = 0.2f; //TimeScale change when enemies are struck by the weapon.

        [FoldoutGroup("Weapon Effects")] public GameObject primaryEffect, secondaryEffect;
    }
}