using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace SmallTimeRogue.Player
{
    public class PlayerGUI : MonoBehaviour
    {
        [ReadOnly] public HealthComponent hc;
        [ReadOnly] public PlayerBody pb;

        [FoldoutGroup("Required")] public Image dashBarFill;
        [FoldoutGroup("Required")] public CanvasGroup dashBar;
        private float _timeSinceDashBarUpdate;

        private void OnValidate()
        {
            if(hc == null) { hc = GetComponent<HealthComponent>(); }
            if(pb == null) { pb = GetComponent<PlayerBody>(); }
        }
        private void Update()
        {
            float time = Time.realtimeSinceStartup;
            
            if(pb.dashCooldownRemaining > 0) { UpdateDashBar(pb.dashCooldown, pb.dashCooldownRemaining); }
            else if (time - _timeSinceDashBarUpdate > 2f) { dashBar.alpha = 0; }
        }

        public void UpdateDashBar(float dashCD, float dashMaxCD)
        {
            dashBar.alpha = 1;
            _timeSinceDashBarUpdate = Time.realtimeSinceStartup;
            float fillBarSize = dashBarFill.rectTransform.rect.width;
            dashBarFill.fillAmount = 1f - Mathf.RoundToInt(fillBarSize * (dashMaxCD / dashCD)) / fillBarSize;
        }
    }
}