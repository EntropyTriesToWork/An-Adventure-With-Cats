using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Enemy
{
    [System.Serializable]
    public struct EnemyStats
    {
        [FoldoutGroup("Enemy Stats")] public int damage;
        [FoldoutGroup("Enemy Stats")] public float visionRange;
        [FoldoutGroup("Enemy Stats")] public float attackRange;
        [FoldoutGroup("Enemy Stats")] public float attackCooldown;
        [FoldoutGroup("Enemy Stats")] public float stateCheckIntervals;
        [FoldoutGroup("Enemy Stats")] public float jumpCooldown;
        [FoldoutGroup("Enemy Stats")] public Vector2 bodySize;
        [FoldoutGroup("Enemy Stats")] public Vector2 coinsDropped;
    }
}