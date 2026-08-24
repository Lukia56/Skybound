using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector3 _velocity;

    private Rigidbody2D _myRigidbody = null;

    private InputAction _moveInput = null;
    private InputAction _jumpInput = null;

    private const float _MOVE_FORCE = 10.0f;
    private const float _JUMP_FORCE = 300.0f;

    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        
        _moveInput = InputSystem.actions.FindAction("Move");
        _jumpInput = InputSystem.actions.FindAction("Jump");
    }

    private void FixedUpdate()
    {
        _myRigidbody.AddForce(_velocity);
        _velocity = Vector3.zero;
    }

    private void Update()
    {
        _velocity.x += _moveInput.ReadValue<Vector2>().x * _MOVE_FORCE;

        if (_jumpInput.WasPerformedThisFrame())
        {
            _velocity.y = _JUMP_FORCE;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _velocity = Vector3.zero;

        
    }

    public void Dead()
    {

    }

    public void JumpUp(float force)
    {

    }

    public void RechargeDash()
    {

    }
}
