using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue
{
    public class PlayAudioOneShot : MonoBehaviour
    {
        public AudioClip audioClip;
        private void Start()
        {
            GameManager.Instance?.PlayOneShotSFX(audioClip);
        }
    }
}