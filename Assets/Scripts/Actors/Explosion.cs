using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SmallTimeRogue
{
    public class Explosion : MonoBehaviour
    {
        [BoxGroup("Settings")] public float radius;
        [BoxGroup("Settings")] public float force;
        [BoxGroup("Settings")] public DamageInfo damageInfo;
        [BoxGroup("Settings")] public bool negateOriginalVelocity;
        [BoxGroup("Settings")] public LayerMask targetedLayers;
        [BoxGroup("Settings")] public GameObject explosionEffect;

        [Button]
        public virtual void Explode()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetedLayers);
            foreach (Collider2D hit in hits)
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(damageInfo);
            }
            ApplyForces(hits);
            AfterDeath();
        }
        public void ApplyForces(Collider2D[] hits)
        {
            foreach (Collider2D hit in hits)
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    if (negateOriginalVelocity) { rb.velocity = Vector2.zero; }
                    rb.AddForce((hit.transform.position - transform.position).normalized * Mathf.Lerp(force, force / 3f, Vector2.Distance(hit.transform.position, transform.position) / radius), ForceMode2D.Impulse);
                }
            }
        }
        public virtual void AfterDeath()
        {
            GameObject go = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            go.transform.localScale = Vector3.one * radius;
            go.SetActive(true);
            Destroy(gameObject);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}