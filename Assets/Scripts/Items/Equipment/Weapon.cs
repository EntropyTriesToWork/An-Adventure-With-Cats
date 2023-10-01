using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using SmallTimeRogue.Player;
using SmallTimeRogue.Stats;

namespace SmallTimeRogue.Items.Weapons
{
    public abstract class Weapon : Equipment
    {
        public Action OnPrimaryUseAction;
        public Action OnSecondaryUseAction;

        [FoldoutGroup("Read Only")] [ReadOnly] [SerializeField] protected float _primaryCooldown, _secondaryCooldown;

        [FoldoutGroup("Required")] [SerializeField] private WeaponStatsSO _stats;
        [HideInInspector] public WeaponStats stats;
        [HideInInspector] public WeaponController playerCombatController;
        protected WeaponController _weaponController;

        public override void Awake()
        {
            base.Awake();
            stats = _stats.weaponStats;
        }
        private void Start()
        {
            _weaponController = FindObjectOfType<WeaponController>();
            if (_weaponController == null) { Debug.LogError("No WeaponController Found!"); Destroy(gameObject); }
        }
        public virtual void Update()
        {
            if (_primaryCooldown >= 0f) { _primaryCooldown -= Time.deltaTime; }
            if (_secondaryCooldown >= 0f) { _secondaryCooldown -= Time.deltaTime; }
            if (_primaryCooldown + stats.primaryDuration <= stats.primaryCooldown) { _sprite.enabled = true; }
            else { _sprite.enabled = false; }
        }
        public override void Pickup()
        {
            _rb.simulated = false;
            _col.enabled = false;
        }
        public override void Discard()
        {
            _rb.simulated = true;
            _col.enabled = true;
        }
        public override void ShowPopup()
        {
            PopupGUIManager.Instance.ShowWeaponPickupable(this, transform.position + Vector3.up * _col.size.y / 2);
        }
        protected DamageReport DealDamage(IDamageable damageable, float damageMultiplier = 1f)
        {
            DamageInfo damageInfo = new DamageInfo(Mathf.RoundToInt(stats.damage * damageMultiplier), stats.armorPenetration, stats.critChance, stats.critDamage);

            DamageReport report = damageable.TakeDamage(damageInfo);

            return report;
        }
        public bool CanPrimary => _primaryCooldown - stats.primaryDuration <= 0f && _primaryCooldown <= 0f;
        public bool CanSecondary => _secondaryCooldown - stats.secondaryDuration <= 0f && _secondaryCooldown <= 0f;
    }
}