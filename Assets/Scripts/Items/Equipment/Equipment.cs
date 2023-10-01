using SmallTimeRogue.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Equipment : MonoBehaviour, IPickupable
    {
        public bool CanMoveInventory { get => _canPickup && _canDiscard; }
        public bool CanDiscard { get => _canDiscard; }
        public bool CanPickup { get => _canPickup; }
        private bool _canPickup, _canDiscard;

        public PlayerBody PlayerBody { get { if (_playerBody == null) { _playerBody = FindObjectOfType<PlayerBody>(); } return _playerBody; } }

        protected SpriteRenderer _sprite;
        protected Rigidbody2D _rb;
        protected BoxCollider2D _col;
        protected PlayerBody _playerBody;

        private void OnValidate()
        {
            if (_sprite == null) _sprite = GetComponent<SpriteRenderer>();
        }
        public virtual void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _col = GetComponent<BoxCollider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _playerBody = FindObjectOfType<PlayerBody>();
            _canPickup = true;
            _canDiscard = true;
        }

        public abstract void Primary();
        public abstract void Secondary();
        public abstract void ShowPopup();
        public virtual void HidePopup()
        {
            PopupGUIManager.Instance.HideEquipmentInformation();
        }
        public abstract void Pickup();
        public abstract void Discard();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ShowPopup();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            HidePopup();
        }
    }
}