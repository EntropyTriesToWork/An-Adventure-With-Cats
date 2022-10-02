using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SmallTimeRogue
{
    public class OverlapCollider : MonoBehaviour
    {
        [Button]
        public void GetOverlapping(RaycastHit2D[] hits)
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.Cast(Vector2.zero, hits, 0.1f);
        }
    }
}