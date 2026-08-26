using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 物理挙動を行うキャラクターの基底
/// </summary>
public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField]
    protected Vector2 _velocity = Vector2.zero;

    [SerializeField]
    protected bool _onGround = false;

    [SerializeField]
    protected bool _useGravity = true;

    [SerializeField]
    protected float _checkGroundRadius = 0.0f;
    [SerializeField]
    protected float _checkGroundDistance = 0.0f;

    [SerializeField]
    private LayerMask _groundLayerMask;

    protected Rigidbody2D _myRigidbody = null;

    protected virtual void FixedUpdate()
    {
        _myRigidbody.linearVelocityX += _velocity.x / _myRigidbody.mass * Time.fixedDeltaTime;
        _myRigidbody.linearVelocityY += _velocity.y / _myRigidbody.mass * Time.fixedDeltaTime;

        _onGround = CheckGround();

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
                _velocity.y += Physics2D.gravity.y;
            }
        }
    }

    protected virtual void AtLanded()
    {
        _velocity.y = 0.0f;
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

    public void SetForce(float force, Vector2 normal)
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
