using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Enemy
{
    public class Slime : Enemy
    {
        private float _gravity = 10;
        public override void Awake()
        {
            base.Awake();
            _gravity = _rb.gravityScale;
        }
        public override void Attack()
        {

        }

        public override void Chase()
        {
            Jump();
            TurnToFaceTarget();
        }

        public override void Jump()
        {
            if (_nextJumpTime <= 0f)
            {
                _nextJumpTime = _enemyStats.jumpCooldown;

                StartCoroutine(DoJump());
            }
            IEnumerator DoJump()
            {
                float time = 0.5f;
                _rb.gravityScale = 0;
                _rb.AddForce(Vector2.up * _baseStats.jumpForce, ForceMode2D.Impulse);
                _rb.AddForce(DirectionToTarget * _baseStats.moveSpeed, ForceMode2D.Impulse);
                _hc.AddCollisionDamageImmunityTime(0.3f);
                Vector2 dir = DirectionToTarget;
                while (time > 0f)
                {
                    yield return new WaitForFixedUpdate();
                    time -= Time.fixedDeltaTime;
                    _rb.gravityScale = Mathf.Lerp(_gravity, 0f, time / 0.5f);
                    _rb.AddForce(dir * _baseStats.moveSpeed);
                }
            }
        }
        public override void Start()
        {
            base.Start();
            _gravity = _rb.gravityScale;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            DealTouchDamage(collision);
        }

        private void DealTouchDamage(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.collider.GetComponent<IDamageable>().TakeDamage(new DamageInfo() { damage = _enemyStats.damage });
                _attackCooldown = _enemyStats.attackCooldown;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (_attackCooldown <= 0f)
            {
                DealTouchDamage(collision);
            }
        }
    }
}