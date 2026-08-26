using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 物理挙動を行うキャラクターの基底
/// </summary>
public abstract class CharacterBase : MonoBehaviour
{
    [Header("Param")]

    [SerializeField]
    protected float _accel = 0.0f;

    [SerializeField]
    protected float _gravityScale = 1.0f;

    [SerializeField]
    private float _checkGroundRadius = 0.0f;
    [SerializeField]
    private float _checkGroundDistance = 0.0f;

    [SerializeField]
    private float _checkCeilingRadius = 0.0f;
    [SerializeField]
    private float _checkCeilingDistance = 0.0f;

    [SerializeField]
    private LayerMask _groundLayerMask;
    public LayerMask GroundLayerMask { get { return _groundLayerMask; } }

    [Header("Member")]

    [SerializeField]
    protected Vector2 _velocity = Vector2.zero;

    [SerializeField]
    protected Vector2 _targetVelocity = Vector2.zero;

    [SerializeField]
    protected bool _onGround = false;
    [SerializeField]
    protected bool _onCeiling = false;

    [SerializeField]
    protected bool _useGravity = true;

    private Rigidbody2D _myRigidbody = null;

    protected virtual void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        _myRigidbody.linearVelocityX = _velocity.x / _myRigidbody.mass * Time.fixedDeltaTime;
        _myRigidbody.linearVelocityY = _velocity.y / _myRigidbody.mass * Time.fixedDeltaTime;

        _velocity.x = Mathf.MoveTowards(_velocity.x, _targetVelocity.x, _accel);

        _onGround = CheckGround();
        _onCeiling = CheckCeiling();

        if (_onGround)
        {
            // 接地時
            if (_velocity.y < 0.0f)
            {
                AtLanded();
            }
        }
        else
        {
            if (_useGravity)
            {
                _velocity.y += Physics2D.gravity.y * _gravityScale;
            }
            else
            {
                _velocity.y = Mathf.MoveTowards(_velocity.y, _targetVelocity.y, _accel);
            }
        }

        if (_onCeiling)
        {
            if (_velocity.y > 0.0f)
            {
                _velocity.y = 0.0f;
            }
        }
    }

    protected virtual void AtLanded()
    {
        _velocity.y = 0.0f;
    }

    public virtual bool IsPlayer()
    {
        return false;
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + Vector3.down * 0.5f, _checkGroundRadius, Vector2.down, _checkGroundDistance, _groundLayerMask);

        return hit.collider != null;
    }

    private bool CheckCeiling()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + Vector3.down * 0.5f, _checkCeilingRadius, Vector2.up, _checkCeilingDistance, _groundLayerMask);

        return hit.collider != null;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.5f + _checkGroundDistance * Vector3.down, _checkGroundRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.5f + _checkCeilingDistance * Vector3.up, _checkCeilingRadius);
    }

    public virtual void SetForce(float force, Vector2 normal)
    {
        Assert.AreApproximatelyEqual(1.0f, normal.sqrMagnitude);

        _velocity = normal * force;

        Debug.Log("Character // SetForce called");
    }

    public virtual void Dead()
    {
        Debug.Log("Character // 死亡処理が呼ばれました");
    }

    public virtual void RechargeDash()
    {
        Debug.Log("Character // RechargeDash called");
    }
}
