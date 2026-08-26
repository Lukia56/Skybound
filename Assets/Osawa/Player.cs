using UnityEngine;
using UnityEngine.InputSystem;

public class Player : CharacterBase
{
    [Header("Param")]

    [SerializeField]
    private float _moveForce = 0.0f;
    [SerializeField]
    private float _jumpForce = 0.0f;

    [SerializeField]
    private float _minVelocityY = 0.0f;

    [SerializeField]
    private float _jumpCancelThreshold = 0.0f;

    [SerializeField]
    private int _maxDashNum = 0;
    [SerializeField]
    private float _dashForce = 0.0f;
    [SerializeField]
    private float _dashTime = 0.0f;

    [Header("Member")]

    [SerializeField]
    private bool _isJumping;

    [SerializeField]
    private int _dashCount = 0;

    // ダッシュ終了までのタイマー
    [SerializeField]
    private float _dashTimer = 0.0f;

    [SerializeField]
    private bool _isDashing = false;

    private InputAction _moveInput = null;
    private InputAction _jumpInput = null;
    private InputAction _dashInput = null;

    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();

        _moveInput = InputSystem.actions.FindAction("Move");
        _jumpInput = InputSystem.actions.FindAction("Jump");
        _dashInput = InputSystem.actions.FindAction("Dash");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _velocity.y = Mathf.Max(_velocity.y, _minVelocityY);
    }

    protected override void AtLanded()
    {
        base.AtLanded();

        _isJumping = false;
        RechargeDash();
    }

    private void Update()
    {
        Vector2 moveDir = _moveInput.ReadValue<Vector2>();

        // ダッシュ中でなければ水平操作を行う
        if (!_isDashing)
        {
            _velocity.x = _moveForce * moveDir.x;
        }

        JumpProcess();

        DashProcess();
    }

    private void JumpProcess()
    {
        if (_jumpInput.WasPressedThisFrame() && _onGround)
        {
            SetForce(_jumpForce, Vector2.up);

            _isJumping = true;
        }

        if (_jumpInput.WasReleasedThisFrame() && _isJumping && _velocity.y > _jumpCancelThreshold)
        {
            _velocity.y = 0;
            _isJumping = false;
        }
    }

    private void DashProcess()
    {
        Vector2 moveDir = _moveInput.ReadValue<Vector2>();

        if (_dashInput.WasPressedThisFrame() && moveDir != Vector2.zero && CanDash())
        {
            SetForce(_dashForce, moveDir);

            _dashCount--;

            _dashTimer = _dashTime;
            _isDashing = true;

            _useGravity = false;
        }

        if (_dashTimer > 0.0f)
        {
            _dashTimer -= Time.deltaTime;
        }
        else if (_isDashing)
        {
            OnEndDash();
        }
    }

    private void OnEndDash()
    {
        _velocity.y = 0.0f;

        _isDashing = false;
        _useGravity = true;

        // 着地した瞬間にのみ回復するため、別途ダッシュ終了時に回復させる
        if (_onGround)
        {
            RechargeDash();
        }
    }

    private bool CanDash()
    {
        return _dashCount > 0;
    }

    public override void Dead()
    {
    }

    public override void RechargeDash()
    {
        _dashCount = _maxDashNum;
    }
}
