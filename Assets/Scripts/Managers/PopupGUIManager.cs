using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using SmallTimeRogue.Items.Weapons;
using System;
using SmallTimeRogue.Items;
using TMPro;
using UnityEngine.InputSystem;
using SmallTimeRogue.Player;

namespace SmallTimeRogue
{
    public class PopupGUIManager : MonoBehaviour
    {
        public static PopupGUIManager Instance;

        [BoxGroup("Equipment")] public CanvasGroup equipmentPickupCanvas;
        [BoxGroup("Equipment")] public TMP_Text equipmentName, equipmentDescription, equipmentStats;

        [BoxGroup("Interaction")] public GameObject interactionIndicator;
        public Weapon selectedWeapon;
        private WeaponController _weaponController;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
        }

        public void ShowInteractionIndicator(Vector3 position)
        {
            interactionIndicator.transform.position = position;
            interactionIndicator.SetActive(true);
        }
        public void HideInteractionIndicator()
        {
            interactionIndicator.SetActive(false);
        }

        public void ShowWeaponPickupable(Weapon equipment, Vector3 position)
        {
            if (equipment == null) { return; }
            ShowInteractionIndicator(position);

            equipmentPickupCanvas.alpha = 1;
            equipmentPickupCanvas.blocksRaycasts = true;
            equipmentPickupCanvas.interactable = true;

            selectedWeapon = equipment;

            ShowEquipmentInformation(equipment);
        }
        public void HideEquipmentInformation()
        {
            HideInteractionIndicator();
            selectedWeapon = null;

            equipmentPickupCanvas.alpha = 0;
            equipmentPickupCanvas.blocksRaycasts = false;
            equipmentPickupCanvas.interactable = false;
        }

        public void ShowEquipmentInformation(Equipment equipment = null)
        {
            Equipment item = null;
            if (equipment != null) { item = equipment; }
            else if (selectedWeapon != null) { item = selectedWeapon; }
            else { Debug.LogError("No Equipment selected!!!"); }

            //TODO Display item stats...
        }
    }
}