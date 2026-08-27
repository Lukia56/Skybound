using UnityEngine;

public class GemReaction : MonoBehaviour
{
    /// <summary>
    /// 削除のカウント
    /// </summary>
    [SerializeField] float _deleteCount = 0.0f;
    /// <summary>
    /// 消去する時間の長さ(秒)
    /// </summary>
    private const float _DELETE_MAX_COUNT = 3.0f;
    /// <summary>
    /// 遠くへ飛ばす座標
    /// </summary>
    private readonly Vector3 _DELETE_POSITION = Vector3.one * (-10000);
    /// <summary>
    /// 自身の初期位置
    /// </summary>
    [SerializeField] private Vector3 _initPos = Vector3.zero;
    private void Start()
    {
        _initPos = transform.position;
    }
    private void Update()
    {
        float deltaTime = Time.deltaTime;

        float deleteCount = _deleteCount;
        _deleteCount -= deltaTime;
        if (deleteCount > 0 && _deleteCount <= 0)
        {
            Init();
        }
    }
    private void Init()
    {
        transform.position = _initPos;
    }
    public void HitReaction()
    {
        _deleteCount = _DELETE_MAX_COUNT;
        transform.position = _DELETE_POSITION;
    }
}
