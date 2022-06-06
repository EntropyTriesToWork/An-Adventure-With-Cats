using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBody : MonoBehaviour
{
    [FoldoutGroup("ReadOnly")] [ReadOnly] public Vector2 rawInputMovement;
    [FoldoutGroup("ReadOnly")] [ReadOnly] public bool grounded;

    [FoldoutGroup("Controls")] public float moveSpeed, jumpForce;

    private Rigidbody2D _rb;
    private BoxCollider2D _col;
    private Vector2 _playerSize;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();

        _playerSize = _col.size;
    }

    public void OnMove(InputValue value)
    {
        rawInputMovement = value.Get<Vector2>();
        rawInputMovement.y = 0;
    }
    public void OnJump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Update()
    {
        CastForGround();
        if (rawInputMovement != Vector2.zero) _rb.velocity += rawInputMovement * moveSpeed * Time.deltaTime * (grounded ? 1f : 0.5f);
    }

    private void CastForGround()
    {
        if (Physics2D.OverlapBox(transform.position + new Vector3(0, -_playerSize.y / 2f, 0), new Vector2(_playerSize.x * 0.9f, 0.1f), 0, LayerMask.GetMask("Ground")))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
