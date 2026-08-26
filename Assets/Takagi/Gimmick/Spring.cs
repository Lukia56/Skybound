using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

public class Spring :GimmickBase
{
    /// <summary>
    /// キャラクターを押す強さの割合
    /// </summary>
    const float _PLAYER_PUSH_RATIO = 1.0f;
 
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        if (_gimmickObj == null) return;
        if (hitType != eHitType.Enter) return;
        // ダッシュの回復
        character.RechargeDash();
        // ギミックの角度から方向を求める
        float radian=_gimmickObj.gameObject.transform.eulerAngles.z*GimmickMath._TO_RADIAN;
        Vector2 forceValue = GimmickMath.RadianToVec(radian);
        // プレイヤーを吹き飛ばす
        character.SetForce(_PUSH_POWER*_PLAYER_PUSH_RATIO, forceValue);
        // ギミックの見た目を動作させる
        _gimmickObj.StartEffect();


        Debug.Log("ギミック発動 : バネ | force : " + hitType + " " + forceValue + " | baseAngle " + radian * GimmickMath._TO_DEGREE);


    }

}
