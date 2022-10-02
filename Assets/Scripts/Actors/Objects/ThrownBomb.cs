using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SmallTimeRogue
{
    public class ThrownBomb : Explosion
    {
        [BoxGroup("Settings")] public float fuseTime = 3f;
        [BoxGroup("Settings")] public Color normalStateColor = Color.white;
        [BoxGroup("Settings")] public Color nearingExplodeStateColor = Color.red;
        [BoxGroup("Required")] public SpriteRenderer bombSprite;
        [BoxGroup("Read Only")] [ReadOnly] public float remainingTime = 0;

        private void Start()
        {
            remainingTime = fuseTime;
        }
        private void Update()
        {
            remainingTime -= Time.deltaTime;
            bombSprite.color = Color.Lerp(nearingExplodeStateColor, normalStateColor, remainingTime / fuseTime);
            if (remainingTime <= 0f)
            {
                Explode();
            }
        }
    }
}