using UnityEngine;

public class Spring :GimmickBase
{
    const float _PLAYER_FORCE_POWER = 150;
    public override void ToPlayerAction(Player character, eHitType hitType)
    {
        if (_gimmickObj == null) return;
        if (hitType != eHitType.Enter) return;
        Vector2 forceValue = RadianToVec(_gimmickObj.transform.rotation.z);
        character.SetForce(_PLAYER_FORCE_POWER, forceValue);
        _gimmickObj.StartEffect();

    }
    /// <summary>
    /// デグリー角からベクトルを取得する関数
    /// </summary>
    /// <param name="radian"></param>
    /// <returns></returns>
    private Vector2 RadianToVec(float radian)
    {
        float x = -Mathf.Sin(radian);
        float y = Mathf.Cos(radian);
        Vector2 vec= new Vector2(x, y);
        return vec;
    }
}
