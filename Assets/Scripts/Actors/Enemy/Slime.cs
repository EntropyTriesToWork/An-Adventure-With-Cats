using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Enemy
{
    public class Slime : Enemy
    {
        public override void Attack()
        {

        }

        public override void Chase()
        {
            Jump();
        }

        public override void Jump()
        {
            if (_nextJumpTime <= 0f)
            {
                _nextJumpTime = enemyStats.jumpCooldown;
                _rb.AddForce(Vector2.up * baseStats.jumpForce, ForceMode2D.Impulse);
                _rb.AddForce(DirectionToTarget * baseStats.moveSpeed, ForceMode2D.Impulse);
                StartCoroutine(AddExtraForces());
            }
            IEnumerator AddExtraForces()
            {
                float time = 0.2f;
                while (time > 0f)
                {
                    yield return new WaitForEndOfFrame();
                    _rb.AddForce(DirectionToTarget * baseStats.moveSpeed);
                    time -= Time.deltaTime;
                }
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            DealTouchDamage(collision);
        }

        private void DealTouchDamage(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.collider.GetComponent<IDamageable>().TakeDamage(new DamageInfo() { damage = enemyStats.damage });
                _attackCooldown = enemyStats.attackCooldown;
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