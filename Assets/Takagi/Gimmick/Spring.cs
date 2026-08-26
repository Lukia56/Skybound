using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

public class Spring :GimmickBase
{
    const float _PLAYER_FORCE_POWER = 100;
    const float _TO_DEGREE = (180.0f / Mathf.PI);
    const float _TO_RADIAN = (Mathf.PI / 180.0f);
    public override void ToCharacterAction(Player character, eHitType hitType)
    {
        if (_gimmickObj == null) return;
        if (hitType != eHitType.Enter) return;
        //float radian = _gimmickObj.gameObject.transform.rotation.z;
        float radian=_gimmickObj.gameObject.transform.eulerAngles.z*_TO_RADIAN;
        Vector2 forceValue = RadianToVec(radian);
        character.SetForce(_PLAYER_FORCE_POWER, forceValue);
        _gimmickObj.StartEffect();

        float angle=Mathf.Atan2(forceValue.y, forceValue.x)*_TO_DEGREE;
        Debug.Log("ギミック発動 : バネ force : " + hitType + " " + forceValue + " Angle " + angle + " baseAngle " + radian * _TO_DEGREE);


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
