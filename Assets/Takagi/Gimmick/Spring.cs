using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

public class Spring :GimmickBase
{
    /// <summary>
    /// キャラクターを押す強さの割合
    /// </summary>
    const float _PLAYER_PUSH_RATIO = 1.0f;
    /// <summary>
    /// 押す力の倍率を求める際に使う角度に応じた力の倍率
    /// </summary>
    const float _PLAYER_PUSH_ROTATE_RATIO = 1.5f;
    /// <summary>
    /// 押す力の倍率を求める際に使う角度の倍率
    /// </summary>
    const float _CALCULATE_RADIAN_RATIO = 1.2f;

 
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        if (_gimmickObj == null) return;
        if (hitType != eHitType.Enter) return;
        
        // ダッシュの回復
        character.RechargeDash();
        // ギミックの角度から方向を求める
        float radian=_gimmickObj.gameObject.transform.eulerAngles.z*GimmickMath._TO_RADIAN;
        Vector2 forceValue = GimmickMath.RadianToVec(radian);

        // 吹き飛ばす力を求める
        // 角度が横になるほど強くなる
        float pushRatio = Mathf.Clamp(
            _PLAYER_PUSH_RATIO * (Mathf.Abs(Mathf.Sin(radian) * _CALCULATE_RADIAN_RATIO) * _PLAYER_PUSH_ROTATE_RATIO),
            1.0f,
            _PLAYER_PUSH_ROTATE_RATIO);
        
        float pushPower = _PUSH_POWER * pushRatio;
        
        // プレイヤーを吹き飛ばす
        character.SetForce(pushPower, forceValue);
        
        // ギミックの見た目を動作させる
        _gimmickObj.StartEffect();


        Debug.Log("ギミック発動 : バネ | force : " + hitType + " " + forceValue + " | baseAngle " + radian * GimmickMath._TO_DEGREE + " | PushPower : " + pushPower);
    }

}
