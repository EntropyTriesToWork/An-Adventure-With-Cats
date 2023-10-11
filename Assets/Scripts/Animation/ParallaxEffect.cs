using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace SmallTimeRogue
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParallaxEffect : MonoBehaviour
    {
        /// 
        /// Uses SpriteRenderer's Length in order to extend or shorten the size of sprites. The sprites needs to be set to Mesh Type of Full Rect in the import settings. 
        /// 

        private Transform _cam;
        private SpriteRenderer _sprite;
        public float speed = 1f; 
        public float startingSize = 100;

        private void Start()
        {
            _cam = FindObjectOfType<CinemachineVirtualCamera>().transform;
            _sprite = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if(_cam != null)
            {
                _sprite.size = new Vector2(speed * _cam.position.x + startingSize, _sprite.size.y);
            }
        }
    }
}