using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector2 _force;

    private bool _isJumping;

    [SerializeField]
    private int _dashCount = 0;

    // ダッシュ終了までのタイマー
    [SerializeField]
    private float _dashTimer = 0.0f;

    private Rigidbody2D _myRigidbody = null;

    private InputAction _moveInput = null;
    private InputAction _jumpInput = null;
    private InputAction _dashInput = null;

    [SerializeField]
    private LayerMask _groundLayerMask;

    private const float _MOVE_FORCE = 100.0f;
    private const float _JUMP_FORCE = 300.0f;

    private const float _MIN_VELOCITY_Y = -400.0f;

    private const float _JUMP_CANCEL_THRESHOLD = 20.0f;

    private const int _MAX_DASH_NUM = 1;
    private const float _DASH_FORCE = 200.0f;
    private const float _DASH_TIME = 0.35f;

    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();

        _moveInput = InputSystem.actions.FindAction("Move");
        _jumpInput = InputSystem.actions.FindAction("Jump");
        _dashInput = InputSystem.actions.FindAction("Sprint");
    }

    private void FixedUpdate()
    {
        Debug.Log(Mathf.Abs(_force.x));
        Debug.Log(Mathf.Abs(_force.y / 100.0f) + 1.0f);
        _myRigidbody.linearVelocityX += (_force.x * (Mathf.Abs(_force.y / 300.0f) + 1.0f)) / _myRigidbody.mass * Time.fixedDeltaTime;
        _myRigidbody.linearVelocityY += _force.y / _myRigidbody.mass * Time.fixedDeltaTime;
    }

    private void Update()
    {
        Vector2 moveDir = _moveInput.ReadValue<Vector2>();

        if (!IsDashing())
        {
            _force.x = _MOVE_FORCE * moveDir.x;
        }

        if (CheckGround())
        {
            _isJumping = false;

            RechargeDash();

            //if (_force.y < 0.0f)
            //{
            //    _force.y = 0.0f;
            //}
        }
        else
        {
            if (!IsDashing())
                _force.y -= 9.81f;
        }

        JumpProcess();

        DashProcess();

        _force.y = Mathf.Max(_force.y, _MIN_VELOCITY_Y);
    }

    private void JumpProcess()
    {
        if (_jumpInput.WasPerformedThisFrame() && CheckGround())
        {
            SetForce(_JUMP_FORCE, Vector2.up);

            _isJumping = true;
        }

        if (_jumpInput.WasReleasedThisFrame() && _isJumping && _myRigidbody.linearVelocityY > 0.0f)
        {
            _myRigidbody.linearVelocityY = 0.0f;
            _force.y = _JUMP_CANCEL_THRESHOLD;
            _isJumping = false;
        }
    }

    private void DashProcess()
    {
        Vector2 moveDir = _moveInput.ReadValue<Vector2>();

        if (_dashInput.WasPerformedThisFrame() && moveDir != Vector2.zero && CanDash())
        {
            SetForce(_DASH_FORCE, moveDir);

            _dashCount--;

            _dashTimer = _DASH_TIME;
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

        _force = normal * force;

        Debug.Log("Player // SetForce called");
    }

    public void RechargeDash()
    {
        _dashCount = _MAX_DASH_NUM;

        Debug.Log("Player // RechargeDash called");
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, _groundLayerMask);

        return hit.collider != null;
    }
}
