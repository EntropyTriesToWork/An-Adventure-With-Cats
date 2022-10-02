using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

namespace SmallTimeRogue.Player
{
    [RequireComponent(typeof(PlayerBody))]
    public class PlayerCombatController : MonoBehaviour
    {
        [BoxGroup("Weapon")] public Transform weaponHandPivot;
        [BoxGroup("Weapon")] public SpriteRenderer weaponSprite;
        [BoxGroup("Weapon")] public OverlapCollider meleeWeaponSwing;

        [BoxGroup("Debug")] public float attackCooldown = 0.1f;

        [BoxGroup("Required")] [Required] public Camera playerCamera;
        [BoxGroup("Read Only")] [ReadOnly] [SerializeField] float _attackCooldown;
        [BoxGroup("Read Only")] [ReadOnly] [SerializeField] bool _primaryActuated;

        PlayerBody _playerBody;
        Vector2 _mouseDelta;
        Vector2 _mousePos;
        PlayerInput _playerInput;

        private void Awake()
        {
            _playerBody = GetComponent<PlayerBody>();
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.actions["Primary"].performed += PlayerCombatController_performed;
            _playerInput.actions["Primary"].canceled += PlayerCombatController_canceled;
        }

        private void PlayerCombatController_canceled(InputAction.CallbackContext obj) { _primaryActuated = false; }
        private void PlayerCombatController_performed(InputAction.CallbackContext obj) { _primaryActuated = true; }

        private void Update()
        {
            if (_attackCooldown >= 0f) { _attackCooldown -= Time.deltaTime; }
            else { weaponSprite.enabled = true; }
        }
        private void FixedUpdate()
        {
            //if (_playerBody.rawInputMovement.x != 0) { weaponHandPivot.localScale = new Vector3(_playerBody.rawInputMovement.x, 1, 1); }
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = -10;
            _mousePos = playerCamera.ScreenToWorldPoint(mousePos);
        }
        public void OnPrimary()
        {
            if (_attackCooldown >= 0f) { return; }

            _attackCooldown = attackCooldown;
            weaponSprite.enabled = false;
            OverlapCollider oc = Instantiate(meleeWeaponSwing, weaponSprite.transform);
            //oc.transform.localScale = new Vector3(1, _mousePos.x <= transform.position.x ? -1 : 1);
            RaycastHit2D[] hits = new RaycastHit2D[10];
            oc.GetOverlapping(hits);

            int totalHits = 0;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    HealthComponent hc = hit.collider.GetComponent<HealthComponent>();
                    if (hc != null)
                    {
                        hc.TakeDamage(new DamageInfo() { damage = 10 });
                        hc.KnockBack((hit.collider.transform.position - transform.position).normalized * 10f);

                        totalHits++;
                    }
                }
            }
            if (totalHits > 0) { _playerBody.FreezeFrame(0.1f); }
        }

        public void OnLook(InputValue value)
        {
            if (value.Get<Vector2>() != Vector2.zero) { _mouseDelta = value.Get<Vector2>(); }
            weaponHandPivot.localScale = _mousePos.x <= transform.position.x ? new Vector3(-1, 1, 1) : Vector3.one;
        }
    }
}