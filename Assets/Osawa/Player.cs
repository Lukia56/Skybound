using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : CharacterBase
{
    enum State
    {
        Idle,
        Run,
        Jump,
        Fall,
        Dash,
        Dead,
    }

    [Header("Param")]

    [SerializeField]
    private float _moveForce = 0.0f;
    [SerializeField]
    private float _jumpForce = 0.0f;

    [SerializeField]
    private Vector2 _wallJumpForce = Vector2.zero;

    [SerializeField]
    private float _minVelocityY = 0.0f;

    [SerializeField]
    private float _jumpCancelThreshold = 0.0f;

    // 着地判定猶予の設定
    [SerializeField]
    private float _checkGroundBufferRadius = 0.0f;
    [SerializeField]
    private float _checkGroundBufferDistance = 0.0f;

    // 壁ジャンプ可能判定の設定
    [SerializeField]
    private float _checkWallRadius = 0.0f;
    [SerializeField]
    private float _checkWallDistance = 0.0f;
    
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

    [SerializeField]
    private bool _onWall = false;

    [SerializeField]
    private int _wallDir = 0;

    [SerializeField]
    private bool _isDead = false;

    private State _state = State.Idle;

    private InputAction _moveInput = null;
    private InputAction _jumpInput = null;
    private InputAction _dashInput = null;

    private Animator _animator = null;

    protected override void Start()
    {
        base.Start();

        _moveInput = InputSystem.actions.FindAction("Move");
        _jumpInput = InputSystem.actions.FindAction("Jump");
        _dashInput = InputSystem.actions.FindAction("Dash");

        _animator = GetComponent<Animator>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_velocity.y < _minVelocityY)
        {
            _velocity.y = Mathf.MoveTowards(_velocity.y, _minVelocityY, Physics2D.gravity.y);
        }

        _onGroundBuffer = CheckGroundBuffer();
        _onWall = CheckWall();
    }

    protected override void AtLanded()
    {
        base.AtLanded();

        _isJumping = false;
        RechargeDash();
    }

    private void Update()
    {
        if (!_isDead)
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

        UpdateAnimation();
    }

    private void JumpProcess()
    {
        if (_jumpInput.WasPressedThisFrame() && CanJump())
        {
            _velocity.y = _jumpForce;

            _isJumping = true;
        }
        if (_jumpInput.WasPressedThisFrame() && CanWallJump())
        {
            _velocity.x = _wallJumpForce.x * _wallDir;
            _velocity.y = _wallJumpForce.y;

            _isJumping = true;
        }

        if (_jumpInput.WasReleasedThisFrame() && _isJumping && _velocity.y > _jumpCancelThreshold)
        {
            _velocity.y = _jumpCancelThreshold;

            _isJumping = false;
        }
    }

    private bool CanJump()
    {
        return _onGroundBuffer && !_isJumping;
    }

    private bool CanWallJump()
    {
        return _onWall && !_onGroundBuffer;
    }

    private void DashProcess()
    {
        Vector2 moveDir = _moveInput.ReadValue<Vector2>();

        if (_dashCooldownTimer > 0.0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }

        if (_dashInput.WasPressedThisFrame() && moveDir != Vector2.zero && CanDash())
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

    private void UpdateAnimation()
    {
        SetAnimationDirection();

        if (_isDead)
        {
            SetState(State.Dead);
            return;
        }

        if (_isDashing)
        {
            SetState(State.Dash);
            return;
        }

        if (_velocity.y < 0.0f)
        {
            SetState(State.Fall);
            return;
        }

        if (_velocity.y > 0.0f)
        {
            SetState(State.Jump);
            return;
        }

        if (_onGround && !Mathf.Approximately(_velocity.x, 0.0f))
        {
            SetState(State.Run);
            return;
        }

        SetState(State.Idle);
    }

    private void SetAnimationDirection()
    {
        Vector2 moveDir = _moveInput.ReadValue<Vector2>();

        Vector3 scale = transform.localScale;
        if (moveDir.x > 0.0f) scale.x = 1.0f;
        if (moveDir.x < 0.0f) scale.x = -1.0f;

        transform.localScale = scale;
    }

    public override void SetForce(float force, Vector2 normal)
    {
        // ダッシュを解除する
        if (_isDashing)
        {
            OnEndDash();
        }

        base.SetForce(force, normal);
    }

    public override void Dead()
    {
        _isDead = true;
    }

    public override void RechargeDash()
    {
        _dashCount = _maxDashNum;
    }

    private void SetState(State state)
    {
        _state = state;
        _animator.SetInteger("State", (int)state);
    }

    public override bool IsPlayer()
    {
        return true;
    }

    /// <summary>
    /// 着地猶予込みで接地判定
    /// </summary>
    private bool CheckGroundBuffer()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _checkGroundBufferRadius, Vector2.down, _checkGroundBufferDistance, GroundLayerMask);

        return hit.collider != null;
    }

    private bool CheckWall()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + Vector3.down * 0.5f, _checkWallRadius, Vector2.down, _checkWallDistance, GroundLayerMask);

        _wallDir = (int)Mathf.Sign(hit.normal.x);

        return hit.collider != null;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _checkGroundBufferDistance * Vector3.down, _checkGroundBufferRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.5f + _checkWallDistance * Vector3.down, _checkWallRadius);
    }
}
