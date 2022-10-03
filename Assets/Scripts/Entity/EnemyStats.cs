using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Enemy
{
    [System.Serializable]
    public struct EnemyStats
    {
        public int damage;
        public float visionRange;
        public float attackRange;
        public float attackCooldown;
        public float stateCheckIntervals;
        public float jumpCooldown;
        public Vector2 bodySize;
    }
}