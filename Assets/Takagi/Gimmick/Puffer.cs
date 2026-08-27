using System.Collections.Generic;
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
    private const float _PUSH_ANGLE_RIGHT = -70.0f;     // 右方向 少し上方向に
    private const float _PUSH_ANGLE_LEFT = 70.0f;       // 左方向 少し上方向に
    private const float _PUSH_ANGLE_RIGHTUP = -60.0f;   // 右上方向
    private const float _PUSH_ANGLE_LEFTUP = 60.0f;    // 左上方向
    /// <summary>
    /// キャラクターを吹き飛ばす強さ
    /// </summary>
    private const float _PUSH_POWER_UP = 0.8f;          // 上方向
    private const float _PUSH_POWER_HOLIZONTAL = 1.5f;  // 水平方向
    private const float _PUSH_POWER_SLANT = 1.5f;       // 斜め方向

    /// <summary>
    /// 踏みつけた時の力の量
    /// </summary>
    private const float _STEP_POWER = _PUSH_POWER*0.4f;           
    
    /// <summary>
    /// キャラクターを押す際のパラメータ
    /// </summary>
    GimmickMath.CharacterPushParam _pushParam;
    struct PufferObject {
        public GimmickObject gimmickObject;
        public CharacterPuffer characterPuffer;
    }
    /// <summary>
    /// フグの配列
    /// </summary>
    private List<PufferObject> _pufferList;
    private CharacterPuffer _actionPuffer = null;
    private eDirectionFour _hitDirection=eDirectionFour.Invalid;
    /// <summary>
    /// キャラクターに対する挙動
    /// </summary>
    /// <param name="character"></param>
    /// <param name="hitType"></param>
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        if (hitType != eHitType.Enter) return;
        // 今回の発動で使用するフグを取得
        _actionPuffer = GetActionPuffer(_gimmickObj);
        if (_actionPuffer == null) return;

        // キャラクターとの角度を求める
        float hitRadian = GetHitRadian(character.transform.position);
        // ダッシュの回復をさせる
        character.RechargeDash();
        // キャラクターを押す量を求める
        CalculatePushParam(hitRadian);
        // キャラクターを押す
        Vector2 pushVector = GimmickMath.RadianToVec(_pushParam.pushRadian);
        character.SetForce(_pushParam.pushPower, pushVector);
        // オブジェクトに対する処理
        ToObjectAction(_gimmickObj, eHitType.Other);
        Debug.Log("ギミック発動 : フグ");
        if (character.GetComponent<CharacterPuffer>() != null)
        {
        Debug.Log("ギミック発動 : フグフグフグフグフグ");

        }
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
        if (_pushParam == null) _pushParam = new GimmickMath.CharacterPushParam();

        // デグリー角
        float angle = 0.0f;

        float halfDiffer = _PI_HALF - Mathf.Abs(radian);
        // 方向を調べる
        // 横方向なら
        if (Mathf.Abs(halfDiffer) > _PI_HALF * 0.5f)
        {
            if (halfDiffer < 0)
            {
                // 右方向なら右へ
                angle = _PUSH_ANGLE_LEFTUP;
            // 接触方向を設定
            _hitDirection=eDirectionFour.Left;
            _hitDirection=eDirectionFour.Left;
            }
            else
            {
                // 左方向なら左へ
                angle = _PUSH_ANGLE_RIGHTUP;
                // 接触方向を設定
                _hitDirection = eDirectionFour.Right;
            }
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
                // 接触方向を設定
                _hitDirection = eDirectionFour.Up;
            }
            // 下方向なら
            else
            {
                if (halfDiffer < 0)
                {
                    // 右方向なら右へ
                    angle = _PUSH_ANGLE_LEFT;
                    Debug.Log("フグ押すRIGHT");
                    
                }
                else
                {
                    // 左方向なら左へ
                    angle = _PUSH_ANGLE_RIGHT;
                    Debug.Log("フグ押すLEFT");

                }
                // キャラクター押す強さの設定
                _pushParam.pushPower = _PUSH_POWER * _PUSH_POWER_SLANT;
                // 接触方向を設定
                _hitDirection= eDirectionFour.Down;
            }
        }
        // デグリー角をラジアン角に変換
        _pushParam.pushRadian=angle*GimmickMath._TO_RADIAN;
    }

    public override void ToObjectAction(GimmickObject gimmickObject, eHitType hitType)
    {
        if (hitType != eHitType.Other) return;
       
        if (_hitDirection == eDirectionFour.Up)
        {
            // 上から踏まれた際は下方向に押す
            _actionPuffer.SetForce(_STEP_POWER, Vector2.down);
        }
        else
        {
            // それ以外は爆破処理
            _actionPuffer.Explosion();
            // 見た目を動作させる
            _gimmickObj.StartEffect();
        }
    }
    private CharacterPuffer GetActionPuffer(GimmickObject gimmickObject)
    {
        CharacterPuffer puffer = null;
        if (_pufferList == null) _pufferList = new List<PufferObject>();
        // 引数のオブジェクトを以前使ったことがあるかどうかを調べる
        for (int i = 0; i < _pufferList.Count; i++)
        {
            if (_pufferList[i].gimmickObject != gimmickObject) continue;
            // 以前使っていたら
            puffer = _pufferList[i].characterPuffer;
        }
        if (puffer == null)
        {
            // 使ったことがなければ

            // 引数のオブジェクトがCharacterPufferを持っているかどうか調べる
            puffer = gimmickObject.GetComponent<CharacterPuffer>();
            // 持っていなければ即時return
            if (puffer == null) return null;
            // 持っているとき
            PufferObject pufferObj;
            pufferObj.gimmickObject = gimmickObject;
            pufferObj.characterPuffer = puffer;
            // 配列に追加
            _pufferList.Add(pufferObj);
        }
        return puffer;
    }

}
