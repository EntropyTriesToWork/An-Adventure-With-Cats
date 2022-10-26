using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using SmallTimeRogue.Items.Weapons;
using System;

namespace SmallTimeRogue.Player
{
    [RequireComponent(typeof(PlayerBody))]
    public class WeaponController : MonoBehaviour
    {
        [BoxGroup("Weapon")] public Transform weaponHandPivot, weaponTransform;
        [BoxGroup("Weapon")] public Weapon currentlyHeldWeapon;

        [BoxGroup("Debug")] public float attackCooldown = 0.1f;
        [BoxGroup("Debug")] public float attackDelay = 0.1f;

        [BoxGroup("Read Only")] [ReadOnly] [SerializeField] bool _primaryActuated, _secondaryActuated;
        [BoxGroup("Read Only")] [ReadOnly] public Vector2 mouseDelta;
        [BoxGroup("Read Only")] [ReadOnly] public Vector2 mousePos;

        PlayerBody _playerBody;
        PlayerInput _playerInput;
        private Camera _playerCamera;

        public Action OnPrimaryDownAction, OnPrimaryReleaseAction;

        private void Awake()
        {
            _playerBody = GetComponent<PlayerBody>();
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.actions["Primary"].performed += Primary_Performed;
            _playerInput.actions["Primary"].canceled += Primary_Canceled;
            _playerInput.actions["Secondary"].performed += Secondary_Performed;
            _playerInput.actions["Secondary"].canceled += Secondary_Canceled;

            _playerCamera = Camera.main;
        }

        private void Secondary_Performed(InputAction.CallbackContext obj) { _secondaryActuated = true; }
        private void Secondary_Canceled(InputAction.CallbackContext obj) { _secondaryActuated = false; }

        private void Primary_Performed(InputAction.CallbackContext obj) { _primaryActuated = true; }
        private void Primary_Canceled(InputAction.CallbackContext obj) { _primaryActuated = false; }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Normal)
            {
                if (currentlyHeldWeapon != null)
                {
                    if (currentlyHeldWeapon.stats.allowsHoldingPrimaryDown && _primaryActuated) OnPrimary();
                    if (currentlyHeldWeapon.stats.allowsHoldingSecondaryDown && _secondaryActuated) OnSecondary();
                    if (_playerBody.rawInputMovement != Vector2.zero) { weaponHandPivot.localScale = _playerBody.rawInputMovement.x <= 0 ? new Vector3(-1, 1, 1) : Vector3.one; }
                }
            }
        }
        private void FixedUpdate()
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = -10;
            this.mousePos = _playerCamera.ScreenToWorldPoint(mousePos);
        }
        public void OnPrimary()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Normal)
            {
                if (currentlyHeldWeapon == null) { return; }
                currentlyHeldWeapon.Primary();
            }
        }
        public void OnSecondary()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Normal)
            {
                if (currentlyHeldWeapon == null) { return; }
                currentlyHeldWeapon.Secondary();
            }
        }
        public void OnInteract()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Normal)
            {
                if (PopupGUIManager.Instance.selectedWeapon != null) { EquipWeapon(PopupGUIManager.Instance.selectedWeapon); }
            }
        }
        public void OnLook(InputValue value)
        {
            if (value.Get<Vector2>() != Vector2.zero) { mouseDelta = value.Get<Vector2>(); }
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon.CanPickup)
            {
                currentlyHeldWeapon = weapon;
                weapon.transform.parent = weaponTransform;
                weapon.transform.localPosition = Vector3.zero;
                weapon.Pickup();
            }
        }
        public void UnequipWeapon()
        {
            if (currentlyHeldWeapon != null)
            {
                if (!currentlyHeldWeapon.CanDiscard) { return; }
                currentlyHeldWeapon.Discard();
                currentlyHeldWeapon.transform.SetParent(null);
                currentlyHeldWeapon = null;
            }
        }
    }
}