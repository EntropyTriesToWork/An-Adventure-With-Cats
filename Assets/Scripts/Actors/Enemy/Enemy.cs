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

        [BoxGroup("Stats")] [Required] public EnemyStatsSO baseStats;

        [BoxGroup("Effects")] [Required] public GameObject activationEffect;

        [FoldoutGroup("Read Only")] [SerializeField] [ReadOnly] protected EntityStats _baseStats;
        [FoldoutGroup("Read Only")] [SerializeField] [ReadOnly] protected EnemyStats _enemyStats;
        [FoldoutGroup("Read Only")] [ReadOnly] public Transform target;
        [FoldoutGroup("Read Only")] [ReadOnly] public bool activated;
        [FoldoutGroup("Read Only")] [ReadOnly] [SerializeField] protected float _timeToNextStateCheck, _attackCooldown, _nextJumpTime;
        protected HealthComponent _hc;
        protected Rigidbody2D _rb;

        #region Messages
        private void FixedUpdate()
        {
            if (GameManager.Instance == null) { return; }
            if (GameManager.Instance.CurrentGameState == GameState.Normal)
            {
                CheckState();
            }
        }
        public virtual void Update()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Normal)
            {
                if (_attackCooldown >= 0f) { _attackCooldown -= Time.deltaTime; }
            }
        }
        public virtual void Start()
        {
            target = DungeonManager.Instance?.Player.transform;
        }
        public virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _hc = GetComponent<HealthComponent>();
            _hc.OnDeathAction += Death;

            _timeToNextStateCheck = 0;
            _attackCooldown = 0;

            _baseStats = baseStats.baseStats;
            _enemyStats = baseStats.enemyStats;

            _hc.SetHealth(_baseStats.health, _baseStats.health);
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
            go.transform.localPosition = Vector3.up * _enemyStats.bodySize.y;
        }
        public virtual void CheckState()
        {
            if (_hc.Dead) { return; }
            if (_timeToNextStateCheck >= 0f) { _timeToNextStateCheck -= Time.fixedDeltaTime; }
            if (_nextJumpTime >= 0f) { _nextJumpTime -= Time.fixedDeltaTime; }

            if (_timeToNextStateCheck <= 0f)
            {
                if (target == null)
                {
                    target = DungeonManager.Instance?.Player.transform;
                    if (target == null) { _timeToNextStateCheck = _enemyStats.stateCheckIntervals; return; }
                }
                if (!activated) { Idle(); _timeToNextStateCheck = _enemyStats.stateCheckIntervals; return; }
                if (HorizontalDistanceToTarget >= _enemyStats.attackRange) { Chase(); return; }
                else if (!IsTargetInAttackRange) { Jump(); return; }
                if (IsTargetInAttackRange && _attackCooldown <= 0f) { Attack(); _attackCooldown = _enemyStats.attackCooldown; return; }
                Debug.LogWarning("No states were triggered!");
                _timeToNextStateCheck = _enemyStats.stateCheckIntervals;
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
            DropCoins();
            gameObject.SetActive(false);
        }

        protected void DropCoins()
        {
            GameManager.Instance?.SpawnCoins(Mathf.RoundToInt(Random.Range(_enemyStats.coinsDropped.x, _enemyStats.coinsDropped.y)), transform.position);
        }

        public virtual void Jump()
        {
            if (_nextJumpTime <= 0f)
            {
                _nextJumpTime = _enemyStats.jumpCooldown;
                _rb.AddForce(Vector2.up * _baseStats.jumpForce, ForceMode2D.Impulse);
            }
        }
        public void TurnToFaceTarget()
        {
            transform.localScale = new Vector3(DirectionToTarget.x > 0 ? 1 : -1, 1, 1);
        }
        #endregion

        #region Getters
        public Vector2 DirectionToTarget => (target.position - transform.position).normalized;
        public Vector2 HorizontalDirectionToTarget => new Vector2(target.position.x, 0) - new Vector2(transform.position.x, 0);
        public float DistanceToTarget => Vector2.Distance(target.position, transform.position);
        public float HorizontalDistanceToTarget => Vector2.Distance(new Vector2(target.position.x, 0), new Vector2(transform.position.x, 0));
        public bool IsTargetVisible => DistanceToTarget <= _enemyStats.visionRange;
        public bool IsTargetInAttackRange => DistanceToTarget <= _enemyStats.attackRange;
        public bool IsWallBlocking => Physics2D.CircleCast(transform.position, _enemyStats.bodySize.y / 2.1f, HorizontalDirectionToTarget, 1f, LayerMask.GetMask("Ground"));
        public RaycastHit2D CastForLineOfSight => Physics2D.Raycast(transform.position, DirectionToTarget, _enemyStats.visionRange, LayerMask.GetMask("Player"));
        #endregion

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _enemyStats.attackRange);
        }
    }
}