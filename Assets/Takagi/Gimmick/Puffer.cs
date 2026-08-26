using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// キャラクターを吹き飛ばしてダッシュ回復をさせる
/// キャラクターとぶつかった角度に応じて挙動が変わる
/// </summary>
public class Puffer:GimmickBase
{
    private const float _PI_HALF = Mathf.PI * 0.5f;
    /// <summary>
    /// キャラクターを吹き飛ばす角度
    /// </summary>
    private const float _PUSH_ANGLE_UP = 0.0f;          // 上方向
    private const float _PUSH_ANGLE_RIGHT = -90.0f;     // 右方向
    private const float _PUSH_ANGLE_LEFT = 90.0f;       // 左方向
    private const float _PUSH_ANGLE_RIGHTUP = -45.0f;   // 右上方向
    private const float _PUSH_ANGLE_LEFTTUP = 45.0f;    // 左上方向
    /// <summary>
    /// キャラクターを吹き飛ばす強さ
    /// </summary>
    private const float _PUSH_POWER_UP = 0.8f;          // 上方向
    private const float _PUSH_POWER_HOLIZONTAL = 1.0f;  // 水平方向
    private const float _PUSH_POWER_SLANT = 1.0f;       // 斜め方向

    /// <summary>
    /// キャラクターを押す際のパラメータ
    /// </summary>
    CharacterPushParam _pushParam;

    /// <summary>
    /// キャラクターに対する挙動
    /// </summary>
    /// <param name="character"></param>
    /// <param name="hitType"></param>
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        // キャラクターとの角度を求める
        float hitRadian = GetHitRadian(character.transform.position);
        // ダッシュの回復をさせる
        character.RechargeDash();
        // キャラクターを押す量を求める
        CalculatePushParam(hitRadian);
        // キャラクターを押す
        character.SetForce(_pushParam.pushPower, _pushParam.pushVec);
    }
    /// <summary>
    /// 自身と指定座標の角度を求める
    /// </summary>
    private float GetHitRadian(float x,float y)
    {
        float radian = 0.0f;
        // ラジアン角を求める
        radian = Mathf.Atan2(y - _gimmickObj.transform.position.y, x - _gimmickObj.transform.position.x);
        return radian;
    }
    private float GetHitRadian(Vector3 otherPos)
    {
        return GetHitRadian(otherPos.x, otherPos.y);
    }
    private float GetHitAngle(Vector2 otherPos)
    {
        return GetHitRadian(otherPos.x, otherPos.y);
    }
    /// <summary>
    /// キャラクターを押す角度を求める
    /// </summary>
    private void CalculatePushParam(float radian)
    {
        // nullなら生成
        if (_pushParam == null) _pushParam = new CharacterPushParam();

        // デグリー角
        float angle = 0.0f;

        float halfDiffer = _PI_HALF - Mathf.Abs(radian);
        // 方向を調べる
        // 横方向なら
        if (Mathf.Abs(halfDiffer) > _PI_HALF * 0.5f)
        {
            // 右方向なら右へ、左方向なら左へ
            angle = (halfDiffer<0)?_PUSH_ANGLE_RIGHTUP:_PUSH_ANGLE_LEFTTUP;
            // キャラクター押す強さの設定
            _pushParam.pushPower = _PUSH_POWER * _PUSH_POWER_HOLIZONTAL;
        }
        else
        {
            // 上方向なら
            if (radian >= 0)
            {
                // 上へ
                angle = _PUSH_ANGLE_UP;
                // キャラクター押す強さの設定
                _pushParam.pushPower = _PUSH_POWER * _PUSH_POWER_UP;
            }
            // 下方向なら
            else
            {
                // 右方向なら右上へ、左方向なら左上へ
                angle = (halfDiffer < 0) ? _PUSH_ANGLE_RIGHTUP : _PUSH_ANGLE_LEFTTUP;
                // キャラクター押す強さの設定
                _pushParam.pushPower = _PUSH_POWER * _PUSH_POWER_SLANT;
            }
        }
        // デグリー角をラジアン角に変換
        _pushParam.pushVec=GimmickMath.RadianToVec(angle*GimmickMath._TO_RADIAN);
    }
}
