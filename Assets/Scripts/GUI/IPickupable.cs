using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Items
{
    public interface IPickupable
    {
        public abstract void ShowPopup();
        public abstract void HidePopup();
        public abstract void Pickup();
        public abstract void Discard();
    }
}