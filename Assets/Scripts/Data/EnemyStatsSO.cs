using Sirenix.OdinInspector;
using SmallTimeRogue.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Stats
{
    [CreateAssetMenu(menuName = "Data/Enemy Stats")]
    public class EnemyStatsSO : ScriptableObject
    {
        [BoxGroup("Stats")] public EntityStats baseStats;
        [BoxGroup("Stats")] public EnemyStats enemyStats;
    }
}