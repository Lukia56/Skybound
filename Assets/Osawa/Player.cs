using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
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
    private Vector2 _velocity;

    [SerializeField]
    private bool _isJumping;

    [SerializeField]
    private int _dashCount = 0;

    // ダッシュ終了までのタイマー
    [SerializeField]
    private float _dashTimer = 0.0f;

    private bool _onGround = false;

    [SerializeField]
    private float _checkGroundRadius = 0.0f;
    [SerializeField]
    private float _checkGroundDistance = 0.0f;

    private Rigidbody2D _myRigidbody = null;

    private InputAction _moveInput = null;
    private InputAction _jumpInput = null;
    private InputAction _dashInput = null;

    [SerializeField]
    private LayerMask _groundLayerMask;

    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();

        _moveInput = InputSystem.actions.FindAction("Move");
        _jumpInput = InputSystem.actions.FindAction("Jump");
        _dashInput = InputSystem.actions.FindAction("Sprint");
    }

    private void FixedUpdate()
    {
        _myRigidbody.linearVelocityX += _velocity.x / _myRigidbody.mass * Time.fixedDeltaTime;
        _myRigidbody.linearVelocityY += _velocity.y / _myRigidbody.mass * Time.fixedDeltaTime;

        _onGround = CheckGround();

        if (_onGround)
        {
            // 接地時には垂直速度をリセットする
            if (_velocity.y < 0.0f)
            {
                _velocity.y = 0.0f;
                _isJumping = false;
            }
        }
        else
        {
            // ダッシュ中でなければ重力の影響を受ける
            if (!IsDashing())
            {
                _velocity.y += Physics2D.gravity.y;
            }
        }

        _velocity.y = Mathf.Max(_velocity.y, _minVelocityY);
    }

    private void Update()
    {
        Vector2 moveDir = _moveInput.ReadValue<Vector2>();

        // ダッシュ中でなければ水平操作を行う
        if (!IsDashing())
        {
            _velocity.x = _moveForce * moveDir.x;
        }

        if (_onGround)
        {
            RechargeDash();
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
        }

        if (IsDashing())
        {
            _dashTimer -= Time.deltaTime;
        }
    }

    private bool CanDash()
    {
        return _dashCount > 0;
    }

    private bool IsDashing()
    {
        return _dashTimer > 0.0f;
    }

    public void Dead()
    {
        Debug.Log("Player // 死亡処理が呼ばれました");
    }

    public void SetForce(float force, Vector2 normal)
    {
        Assert.AreApproximatelyEqual(1.0f, normal.sqrMagnitude);

        _velocity = normal * force;

        Debug.Log("Player // SetForce called");
    }

    public void RechargeDash()
    {
        _dashCount = _maxDashNum;

        Debug.Log("Player // RechargeDash called");
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _checkGroundRadius, Vector2.down, _checkGroundDistance, _groundLayerMask);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + _checkGroundDistance * Vector3.down, _checkGroundRadius);
    }
}
