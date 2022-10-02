using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue
{
    public interface IDamageable
    {
        public DamageReport TakeDamage(DamageInfo damageInfo);
    }
}