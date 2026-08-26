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

    // 着地判定猶予の設定
    [SerializeField]
    private float _checkGroundBufferRadius = 0.0f;
    [SerializeField]
    private float _checkGroundBufferDistance = 0.0f;

    [SerializeField]
    private int _maxDashNum = 0;
    [SerializeField]
    private float _dashForce = 0.0f;
    [SerializeField]
    private float _dashDuration = 0.0f;
    [SerializeField]
    private float _dashCooldownTime = 0.0f;

    [Header("Member")]

    [SerializeField]
    private bool _isJumping;

    [SerializeField]
    private int _dashCount = 0;

    // ダッシュ終了までのタイマー
    [SerializeField]
    private float _dashTimer = 0.0f;

    [SerializeField]
    private float _dashCooldownTimer = 0.0f;

    [SerializeField]
    private bool _isDashing = false;

    private bool _onGroundBuffer = false;

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

        if (_velocity.y < _minVelocityY)
        {
            _velocity.y = Mathf.MoveTowards(_velocity.y, _minVelocityY, Physics2D.gravity.y);
        }

        _onGroundBuffer = CheckGroundBuffer();
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
            _targetVelocity.x = _moveForce * moveDir.x;
        }

        JumpProcess();

        DashProcess();
    }

    private void JumpProcess()
    {
        if (_jumpInput.WasPressedThisFrame() && _onGroundBuffer)
        {
            _velocity.y = _jumpForce;

            _isJumping = true;
        }

        if (_jumpInput.WasReleasedThisFrame() && _isJumping && _velocity.y > _jumpCancelThreshold)
        {
            _velocity.y = _jumpCancelThreshold;

            _isJumping = false;
        }
    }

    private void DashProcess()
    {
        Vector2 moveDir = _moveInput.ReadValue<Vector2>();

        if (_dashCooldownTimer > 0.0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }

        if (_dashInput.IsPressed() && moveDir != Vector2.zero && CanDash())
        {
            SetForce(_dashForce, moveDir);

            _dashCount--;

            _dashTimer = _dashDuration;
            _dashCooldownTimer = _dashCooldownTime;

            _isDashing = true;
            _isJumping = false;

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
        return _dashCount > 0 && _dashCooldownTimer <= 0.0f;
    }

    public override void Dead()
    {
    }

    public override void RechargeDash()
    {
        _dashCount = _maxDashNum;
    }

    /// <summary>
    /// 着地猶予込みで接地判定
    /// </summary>
    private bool CheckGroundBuffer()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _checkGroundBufferRadius, Vector2.down, _checkGroundBufferDistance, GroundLayerMask);

        return hit.collider != null;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _checkGroundBufferDistance * Vector3.down, _checkGroundBufferRadius);
    }
}
