using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// フグのオブジェクト
/// 削除する際には遠くに飛ばすことで削除したことにする
/// </summary>
public class CharacterPuffer : CharacterBase
{
    /// <summary>
    /// ヘッダー
    /// </summary>
    [Header("Puffer")]
    /// <summary>
    /// アクティブかどうか
    /// </summary>
    public bool isActive { get; private set; } = false;
    /// <summary>
    /// 初期座標　削除したものを元に戻す際に用いる
    /// </summary>
    [SerializeField] private Vector3 _initPos = Vector3.zero;
    /// <summary>
    /// 削除中のカウント　0になったら復活する
    /// </summary>
    [SerializeField] private float _deleteCount = 0.0f;
    /// <summary>
    /// 削除する時間
    /// </summary>
    private const float _DELETE_COUNT = 3.0f;
    /// <summary>
    /// 削除する際に飛ばす座標
    /// </summary>
    private readonly Vector3 _DELETE_POSITION = Vector3.one * -10000.0f;

    [SerializeField] private CollisionCensor _vertilal = null;
    [SerializeField] private CollisionCensor _holizontal = null;
    /// <summary>
    /// 押された時のベクトル
    /// </summary>
    [SerializeField] private Vector2 _pushVector = Vector2.zero;
    [SerializeField] private float _pushSpeed = 0.0f;
    /// <summary>
    /// 押されてから動きを止めるまでの時間
    /// </summary>
    private const float _PUSH_MAX_SECOND = 0.5f;
    /// <summary>
    /// 押された直後の経過時間
    /// </summary>
    [SerializeField] private float _pushCount = 0.0f;
    private const float _PUSH_POWER_NORMALIZE = 0.025f;
    protected override void Start()
    {
        // 初期座標を取得
        _initPos = transform.position;
        Init();
    }
    protected override void FixedUpdate()
    {
    }

    private void Update()
    {
        float deltaTime= Time.deltaTime;
        // 削除カウントをカウントダウン
        float deleteCount = _deleteCount - deltaTime;
        // カウントが0になった瞬間
        if (deleteCount <= 0.0f)
        {
            if (_deleteCount > 0)
            {
                // 初期化処理を行う
                Init();
            }
            deleteCount = 0.0f;
        }
        
        _deleteCount = deleteCount;

        UpdatePosition();
    }
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Init()
    {
        transform.position = _initPos;
        isActive = true;
        _deleteCount = 0.0f;
        _pushCount = _PUSH_MAX_SECOND;
        _pushVector = Vector2.zero;
        _pushSpeed = 0.0f;
    }
    /// <summary>
    /// 爆発処理
    /// </summary>
    public void Explosion()
    {
        // フラグ更新
        isActive = false;
        // 座標を遠くに飛ばすことで消したことにする
        transform.position = _DELETE_POSITION;
        // 削除のカウントをリセット
        _deleteCount = _DELETE_COUNT;
    }
    public override void SetForce(float force, Vector2 normal)
    {
        _pushVector = normal;
        _pushSpeed = force * _PUSH_POWER_NORMALIZE;
        _pushCount= _PUSH_MAX_SECOND;
    }
    private void UpdatePosition()
    {
        // デルタタイムをキャッシュ
        float deltaTime = Time.deltaTime;
        _pushCount -= deltaTime;
        if (_pushCount < 0.0f)
        {
        // 一定時間動いたら即時return
            _pushCount = 0.0f;
            return;
        }
        // 垂直方向のセンサーがマップのフィールドに接触していたら
        if (_vertilal&& _vertilal.isHit)
        {
            // 垂直方向の移動量をリセット
            _pushVector.y = 0.0f;
        }
        // 水平方向のセンサーがマップのフィールドに接触していたら
        if (_holizontal && _holizontal.isHit)
        {
            // 水平方向の移動量をリセット
            _pushVector.x = 0.0f;
        }
        // 移動量をもとに動く
        Vector3 position = transform.position;
        float _pushRatio = _pushCount / _PUSH_MAX_SECOND;
        position.x += _pushVector.x * _pushSpeed * deltaTime * _pushRatio;
        position.y += _pushVector.y * _pushSpeed * deltaTime * _pushRatio;
        transform.position = position;
    }
}

