using SmallTimeRogue.Items;
using SmallTimeRogue.Items.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        public void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
        }
    }
}