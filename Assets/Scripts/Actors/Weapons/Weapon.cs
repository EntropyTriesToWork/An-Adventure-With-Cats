using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace SmallTimeRogue.Items.Weapons
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Weapon : MonoBehaviour
    {
        [FoldoutGroup("Required")] [Required] public WeaponSO weaponStats;
        [FoldoutGroup("Required")] [Required] public SpriteRenderer weaponSprite;

        public Action OnPrimaryUse;
        public Action OnSecondaryUse;

        [FoldoutGroup("Read Only")] [ReadOnly] public float attackCooldown;

        private WeaponStats _ws;

        private void OnValidate()
        {
            if (weaponSprite == null) weaponSprite = GetComponent<SpriteRenderer>();
        }
        public virtual void Update()
        {
            if (attackCooldown > 0f) { attackCooldown -= Time.deltaTime; }
        }
        private void Awake()
        {
            _ws = weaponStats.weaponStats;
        }

        protected float _attackCooldown;

        public abstract void Primary();
        public abstract void Secondary();

        protected void DoFreezeFrame() => GameEffectsManager.Instance.FreezeFrame(_ws.freezeFrameDuration, _ws.freezeFrameScale);
        protected DamageReport DealDamage(IDamageable damageable)
        {
            DamageInfo damageInfo = new DamageInfo()
            {
                damage = _ws.damage,
                armorPenetration = _ws.armorPenetration,
                critChance = _ws.critChance,
                critDamage = _ws.critDamage
            };

            DamageReport report = damageable.TakeDamage(damageInfo);

            return report;
        }
    }
}