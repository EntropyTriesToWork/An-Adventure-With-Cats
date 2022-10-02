using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using SmallTimeRogue.Stats;
using System;

namespace SmallTimeRogue.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerBody : MonoBehaviour
    {
        #region Variables
        [FoldoutGroup("ReadOnly")] [ReadOnly] public Vector2 rawInputMovement, lastFrameInput;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public bool grounded;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public bool canMove = true;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public bool canJump = true;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public int jumpsRemaining = 1;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public EntityStat gravity = new EntityStat();
        [FoldoutGroup("ReadOnly")] [ReadOnly] public float dashCooldownRemaining;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public float dashesRemaining;

        [FoldoutGroup("Controls")] public float moveSpeed, jumpForce, dashForce;
        [FoldoutGroup("Controls")] public int jumps, dashes;
        [FoldoutGroup("Controls")] public float dashCooldown, dashTime;

        [FoldoutGroup("Audio")] public AudioClip jumpSFX, dashSFX;
        [FoldoutGroup("Audio")] public AudioSource audioSource;

        [FoldoutGroup("Required")] [Required] public SpriteRenderer bodySprite;

        private Rigidbody2D _rb;
        private BoxCollider2D _col;
        private Vector2 _playerSize;
        private float _lastDashTime;
        private HealthComponent _hc;

        private Coroutine _jumpGravityAffector = null;
        private Coroutine _dashControl = null;
        private Coroutine _freezeFrame = null;
        #endregion

        #region Events
        public Action OnDashAction;
        public Action OnJumpAction;
        #endregion

        #region Messages
        private void Awake()
        {
            canJump = true;
            canMove = true;
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<BoxCollider2D>();

            _playerSize = _col.size;

            gravity.BaseValue = _rb.gravityScale;
            _hc = GetComponent<HealthComponent>();
        }
        public void Start()
        {
            _hc.OnHealthChange += UpdateHealthBar;
        }
        private void OnDisable()
        {
            _hc.OnHealthChange -= UpdateHealthBar;
        }
        private void Update()
        {
            CastForGround();
            if (rawInputMovement != Vector2.zero && canMove)
            {
                _rb.velocity += rawInputMovement * moveSpeed * Time.deltaTime * (grounded ? 1f : 0.75f);
                lastFrameInput = rawInputMovement;
            }

            if (dashesRemaining < dashes)
            {
                dashCooldownRemaining -= Time.deltaTime;

                if (dashCooldownRemaining <= 0f)
                {
                    dashesRemaining++;
                    dashCooldownRemaining = dashCooldown;
                }
            }
        }
        #endregion

        public void FreezeFrame(float time)
        {
            if (_freezeFrame != null) { StopCoroutine(_freezeFrame); }
            _freezeFrame = StartCoroutine(DoFreezeFrame(time));

            IEnumerator DoFreezeFrame(float time)
            {
                Time.timeScale = 0.2f;
                yield return new WaitForSecondsRealtime(time);
                Time.timeScale = 1f;
            }
        }
        private void UpdateHealthBar() { GameManager.Instance.UpdateHealthBar(_hc.Health, _hc.MaxHealth); }
        #region Input
        public void OnMove(InputValue value)
        {
            rawInputMovement = value.Get<Vector2>();
            rawInputMovement.y = 0;
        }
        public void OnJump()
        {
            if (!canJump) { return; }
            if (grounded) { DoJump(); jumpsRemaining = jumps - 1; }
            else if (jumpsRemaining > 0)
            {
                DoJump();
                jumpsRemaining--;
            }

            void DoJump()
            {
                CancelDash();
                audioSource.PlayOneShot(jumpSFX);
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                if (_jumpGravityAffector != null) { StopCoroutine(_jumpGravityAffector); _jumpGravityAffector = null; }
                _jumpGravityAffector = StartCoroutine(JumpReduceGravity());
            }

            IEnumerator JumpReduceGravity()
            {
                float time = 0.5f;
                _rb.gravityScale = 0;
                while (time > 0f)
                {
                    yield return new WaitForFixedUpdate();
                    time -= Time.fixedDeltaTime;
                    _rb.gravityScale = Mathf.Lerp(gravity.Value, 0f, time / 0.5f);
                }
            }
        }
        public void CancelJump()
        {
            if (_jumpGravityAffector != null) { StopCoroutine(_jumpGravityAffector); _jumpGravityAffector = null; }
        }
        public void CancelDash()
        {
            if (_dashControl != null) { StopCoroutine(_dashControl); _dashControl = null; }
        }
        public void OnDash()
        {
            if (dashesRemaining > 0 && _dashControl == null)
            {
                if (Time.realtimeSinceStartup - _lastDashTime < dashTime + 0.1f)
                {
                    CancelDash();
                }
                CancelJump();

                audioSource.PlayOneShot(dashSFX);
                _lastDashTime = Time.realtimeSinceStartup;
                _rb.velocity = Vector2.zero;
                _rb.AddForce(rawInputMovement * dashForce, ForceMode2D.Impulse);
                dashesRemaining--;
                _dashControl = StartCoroutine(DoDash());
            }

            IEnumerator DoDash()
            {
                _rb.gravityScale = 0;
                float time = dashTime;
                while (time > 0f)
                {
                    GameObject go = Instantiate(bodySprite.gameObject, transform.position, Quaternion.identity);
                    go.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.1f);
                    go.GetComponent<SpriteRenderer>().sortingOrder--;
                    Destroy(go, Time.fixedDeltaTime * 5);
                    time -= Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
                _rb.gravityScale = gravity.Value;
                _dashControl = null;
            }
        }
        #endregion

        #region Physics
        private void CastForGround()
        {
            if (Physics2D.OverlapBox(transform.position + new Vector3(0, -_playerSize.y / 2f, 0), new Vector2(_playerSize.x * 0.95f, 0.1f), 0, LayerMask.GetMask("Ground")))
            {
                grounded = true;
                if (_rb.velocity.y < 1f)
                    jumpsRemaining = jumps;
            }
            else
            {
                grounded = false;
            }
        }
        #endregion
    }
}