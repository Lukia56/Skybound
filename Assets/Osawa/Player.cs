using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector2 _force;

    private bool _isJumping;

    private Rigidbody2D _myRigidbody = null;

    private InputAction _moveInput = null;
    private InputAction _jumpInput = null;

    [SerializeField]
    private LayerMask _groundLayerMask;

    private const float _MOVE_FORCE = 10.0f;
    private const float _JUMP_FORCE = 300.0f;

    private const float _JUMP_CANCEL_THRESHOLD = 20.0f;

    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();

        _moveInput = InputSystem.actions.FindAction("Move");
        _jumpInput = InputSystem.actions.FindAction("Jump");
    }

    private void FixedUpdate()
    {
        _myRigidbody.AddForce(_force);
        _force = Vector2.zero;
    }

    private void Update()
    {
        _force.x = _moveInput.ReadValue<Vector2>().x * _MOVE_FORCE;

        if (_jumpInput.WasPerformedThisFrame() && CheckGround())
        {
            SetForce(_JUMP_FORCE, Vector2.up);

            _isJumping = true;
        }

        if (_jumpInput.WasReleasedThisFrame() && _isJumping && _myRigidbody.linearVelocityY > 0.0f)
        {
            _myRigidbody.linearVelocityY = 0.0f;
            _force.y = _JUMP_CANCEL_THRESHOLD;
        }
    }

    public void Dead()
    {

    }

    public void SetForce(float force, Vector2 normal)
    {
        Assert.AreApproximatelyEqual(1.0f, normal.sqrMagnitude);

        _myRigidbody.linearVelocityY = 0.0f;
        _force = normal * force;
    }

    public void RechargeDash()
    {

    }

    bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, _groundLayerMask);
        
        return hit.collider != null;
    }
}
