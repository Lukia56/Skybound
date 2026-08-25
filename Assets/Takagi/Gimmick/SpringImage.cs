using System;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// バネの板を動かす
/// </summary>
public class SpringImage : GimmickImage
{
    /// <summary>
    /// オブジェクトを押す板
    /// </summary>
    [SerializeField] private Transform _pushBoard=null;
    /// <summary>
    /// 移動先
    /// </summary>
    [SerializeField] private Transform _targetPos = null;
    /// <summary>
    /// 移動量
    /// </summary>
    private Vector2 _moveVec= Vector2.zero;
    /// <summary>
    /// バネ板が動いてからの時間
    /// </summary>
    [SerializeField] private float _effectCount = 0.0f;
    /// <summary>
    /// バネ板の動くスピード
    /// </summary>
    const float _EFFECT_SPEED = 4.0f;
    /// <summary>
    /// 最大カウント
    /// </summary>
    const float _EFFECT_MAX_COUNT = 2.0f;
    [SerializeField] float ratio = 0.0f;
    private void Update()
    {
        float deltaTime=Time.deltaTime;
        // 座標を計算で求める
        CalculatePosition();
        // 効果時間を更新
        _effectCount -= _EFFECT_SPEED*deltaTime;
        _effectCount = Mathf.Clamp(_effectCount, 0, _EFFECT_MAX_COUNT);
    }
    public override void StartActionEffect()
    {
        ResetTransform();
    }
    // バネの板の座標を初期化
    private void ResetTransform()
    {
        if (_pushBoard != null)
        {
            // 板があれば板の座標をリセット
            _pushBoard.position = Vector3.zero;
            if (_targetPos != null)
            {
                // 移動方向があれば取得
                _moveVec = _targetPos.position-transform.position;
            }
        }
        _effectCount = _EFFECT_MAX_COUNT;
    }
    private void CalculatePosition()
    {
        // 座標の割合を取得
        ratio = -(_effectCount - 1.0f)* (_effectCount - 1.0f) + 1.0f;
        // 範囲外の値にならないようクランプ
        ratio=Mathf.Clamp(ratio, 0.0f, 1.0f);
        // 求めた割合をもとに座標を求める
        Vector3 position = _moveVec * ratio;
        _pushBoard.position = transform.position + position;
    }
}
