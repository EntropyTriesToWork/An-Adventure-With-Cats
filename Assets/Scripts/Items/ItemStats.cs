using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Items
{
    [System.Serializable]
    public class ItemStats
    {
        [BoxGroup("Item")] public string name = "ITEM_NAME_MISSING";
        [BoxGroup("Item")] public string description = "ITEM_DESC_MISSING";
        [BoxGroup("Item")] public int dropWeight = 0;
        [BoxGroup("Item")] public Rarity rarity = Rarity.Normal;
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