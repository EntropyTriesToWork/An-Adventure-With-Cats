using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Items
{
    [CreateAssetMenu(menuName = "Data/Weapon")]
    public class WeaponSO : ScriptableObject
    {
        public WeaponStats weaponStats;
    }
}