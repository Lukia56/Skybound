using Unity.VisualScripting;
using UnityEngine;


public class GimmickBase
{
    /// <summary>
    /// 吹きとばす強さの基本
    /// </summary>
    protected const float _PUSH_POWER = 500;



    /// <summary>
    /// 指定したキャラクターに対するアクション
    /// </summary>
    /// <param name="character"></param>
    /// <param name="hitType"></param>
    public virtual void ToCharacterAction(CharacterBase character, eHitType hitType){}
    public void SetGimmickObject(GimmickObject gimmickObj) { this._gimmickObj = gimmickObj; }
    protected GimmickObject _gimmickObj;
    /// <summary>
    /// 指定したオブジェクトに対するアクション
    /// </summary>
    /// <param name="gimmickObject"></param>
    public virtual void ToObjectAction(GimmickObject gimmickObject,eHitType hitType) {}
    /// <summary>
    /// その他のアクション
    /// </summary>
    public virtual void OtherAction(eHitType hitType) { }
}
