using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Stats
{
    [System.Serializable]
    public class ItemStats
    {
        [FoldoutGroup("Item")] public string name = "ITEM_NAME_MISSING";
        [FoldoutGroup("Item")] public string description = "ITEM_DESC_MISSING";
        [FoldoutGroup("Item")] public int dropWeight = 0;
        [FoldoutGroup("Item")] public Rarity rarity = Rarity.Normal;
        [FoldoutGroup("Item")] public Sprite itemSprite;
        [FoldoutGroup("Item")] public GameObject itemPrefab;
    }
    public enum Rarity
    {
        Normal,
        Rare,
        Magic,
        Epic,
        Unique,
        Legendary,
        Divine
    }
}