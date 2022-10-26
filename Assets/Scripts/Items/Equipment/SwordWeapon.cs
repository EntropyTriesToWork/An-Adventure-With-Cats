using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Items.Weapons
{
    public class SwordWeapon : Weapon
    {
        public override void Primary()
        {
            if (_primaryCooldown >= 0f) { return; }

            _primaryCooldown = stats.primaryCooldown;
            OverlapCollider oc = Instantiate(stats.primaryEffect, transform).GetComponent<OverlapCollider>();
            oc.transform.SetParent(null);
            RaycastHit2D[] hits = new RaycastHit2D[10];
            oc.GetOverlapping(hits);

            int totalCrits = 0;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    HealthComponent hc = hit.collider.GetComponent<HealthComponent>();
                    if (hc != null)
                    {
                        DamageReport report = DealDamage(hc);
                        hc.KnockBack((hit.collider.transform.position - transform.position).normalized * 10f);
                        if (report.isCrit) { totalCrits++; }
                    }
                }
            }
            if (totalCrits > 0) { GameEffectsManager.Instance.FreezeFrame(0.1f, 0.1f); }
        }

        public override void Secondary()
        {
            if (_secondaryCooldown >= 0f || !CanPrimary) { return; }

            GameEffectsManager.Instance.FreezeFrame(0.25f, 0.1f);

            _secondaryCooldown = stats.secondaryCooldown;
            OverlapCollider oc = Instantiate(stats.secondaryEffect, transform).GetComponent<OverlapCollider>();
            oc.transform.SetParent(null);

            RaycastHit2D[] hits = new RaycastHit2D[10];
            oc.GetOverlapping(hits);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    HealthComponent hc = hit.collider.GetComponent<HealthComponent>();
                    if (hc != null)
                    {
                        DealDamage(hc, 2f);
                        hc.KnockBack((hit.collider.transform.position - transform.position).normalized * 50f);
                    }
                }
            }
            _playerBody.AddForce(Vector2.up * _playerBody.jumpForce / 2f);
        }
    }
}