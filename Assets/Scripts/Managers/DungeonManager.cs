using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SmallTimeRogue
{
    public class DungeonManager : MonoBehaviour
    {
        public GameObject playerPrefab;

        public static DungeonManager Instance;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }

            Player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
        public GameObject Player { get; private set; }
    }
}