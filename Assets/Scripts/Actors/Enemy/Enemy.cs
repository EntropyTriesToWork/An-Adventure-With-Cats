using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using SmallTimeRogue.Stats;
using SmallTimeRogue.Player;

namespace SmallTimeRogue.Enemy
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Enemy : MonoBehaviour
    {
        [BoxGroup("Stats")] public EntityStats baseStats;
        [BoxGroup("Stats")] public EnemyStats enemyStats;
        [BoxGroup("Stats")] public Vector2 coinsDropped;

        [BoxGroup("Required")] [Required] public GameObject activationEffect;

        [BoxGroup("Read Only")] [ReadOnly] public Transform target;
        [BoxGroup("Read Only")] [ReadOnly] public bool activated;
        [BoxGroup("Read Only")] [ReadOnly] [SerializeField] float _timeToNextStateCheck, _attackCooldown, _nextJumpTime;
        protected HealthComponent _hc;
        protected Rigidbody2D _rb;

        #region Messages
        public virtual void OnEnable()
        {
            _hc = GetComponent<HealthComponent>();
            target = FindObjectOfType<PlayerBody>().transform;
            _hc.SetHealth(baseStats.health, baseStats.health);
            _rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            CheckState();
        }
        private void Awake()
        {
            _hc = GetComponent<HealthComponent>();
            _hc.OnDeathAction += Death;

            _timeToNextStateCheck = 0;
            _attackCooldown = 0;
        }
        private void OnDisable()
        {
            _hc.OnDeathAction -= Death;
        }
        #endregion

        #region State
        public void Activate()
        {
            activated = true;
            GameObject go = Instantiate(activationEffect, transform);
            go.transform.localPosition = Vector3.up * enemyStats.bodySize.y;
        }
        public void CheckState()
        {
            if (_hc.Dead) { return; }
            if (_timeToNextStateCheck >= 0f) { _timeToNextStateCheck -= Time.fixedDeltaTime; }
            if (_attackCooldown >= 0f) { _attackCooldown -= Time.fixedDeltaTime; }
            if (_nextJumpTime >= 0f) { _nextJumpTime -= Time.fixedDeltaTime; }

            if (_timeToNextStateCheck <= 0f)
            {
                if (!activated) { Idle(); _timeToNextStateCheck = enemyStats.stateCheckIntervals; return; }
                if (HorizontalDistanceToTarget >= enemyStats.attackRange) { Chase(); return; }
                else if (!IsTargetInAttackRange) { Jump(); return; }
                if (IsTargetInAttackRange && _attackCooldown <= 0f) { Attack(); _attackCooldown = enemyStats.attackCooldown; return; }
                Debug.LogWarning("No states were triggered!");
                _timeToNextStateCheck = enemyStats.stateCheckIntervals;
            }
        }
        public virtual void Idle()
        {
            if (CastForLineOfSight) { Activate(); }
        }
        public abstract void Chase();
        public abstract void Attack();
        public virtual void Death(DamageReport report)
        {
            GameManager.Instance?.SpawnCoins(Mathf.RoundToInt(Random.Range(coinsDropped.x, coinsDropped.y)), transform.position);
        }
        public virtual void Jump()
        {
            if (_nextJumpTime <= 0f)
            {
                _nextJumpTime = enemyStats.jumpCooldown;
                _rb.AddForce(Vector2.up * baseStats.jumpForce, ForceMode2D.Impulse);
            }
        }
        #endregion

        #region Getters
        public Vector2 DirectionToTarget => (target.position - transform.position).normalized;
        public Vector2 HorizontalDirectionToTarget => new Vector2(target.position.x, 0) - new Vector2(transform.position.x, 0);
        public float DistanceToTarget => Vector2.Distance(target.position, transform.position);
        public float HorizontalDistanceToTarget => Vector2.Distance(new Vector2(target.position.x, 0), new Vector2(transform.position.x, 0));
        public bool IsTargetVisible => DistanceToTarget <= enemyStats.visionRange;
        public bool IsTargetInAttackRange => DistanceToTarget <= enemyStats.attackRange;
        public bool IsWallBlocking => Physics2D.CircleCast(transform.position, enemyStats.bodySize.y / 2.1f, HorizontalDirectionToTarget, 1f, LayerMask.GetMask("Ground"));
        public RaycastHit2D CastForLineOfSight => Physics2D.Raycast(transform.position, DirectionToTarget, enemyStats.visionRange, LayerMask.GetMask("Player"));
        #endregion

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyStats.attackRange);
        }
    }
}