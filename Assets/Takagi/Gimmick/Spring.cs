using UnityEngine;

public class Spring :GimmickBase
{
    const float _TO_RADIAN = Mathf.PI / 180.0f;
    const float _PLAYER_FORCE_POWER = 10;
    public override void ToPlayerAction(Player character, eHitType hitType)
    {
        if (_gimmickObj == null) return;
        if (hitType != eHitType.Enter) return;
        Debug.Log("ギミック発動 : ジャンプ台 " + hitType);
        Vector2 forceValue = AngleToVec(_gimmickObj.transform.rotation.z);
        character.SetForce(_PLAYER_FORCE_POWER, forceValue);
        _gimmickObj.StartEffect();

    }
    /// <summary>
    /// デグリー角からベクトルを取得する関数
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector2 AngleToVec(float angle)
    {
        float radian = angle * _TO_RADIAN;
        float x = Mathf.Cos(radian);
        float y = Mathf.Sin(radian);
        return new Vector2(x, y);
    }
}
