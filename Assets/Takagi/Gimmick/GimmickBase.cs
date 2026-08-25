using UnityEngine;


public class GimmickBase
{
    /// <summary>
    /// 指定したプレイヤーに対するアクション
    /// </summary>
    /// <param name="character"></param>
    /// <param name="hitType"></param>
    public virtual void ToPlayerAction(Player character, eHitType hitType){}
    /// <summary>
    /// 指定したオブジェクトに対するアクション
    /// </summary>
    /// <param name="gimmickObject"></param>
    public virtual void ToObjectAction(GimmickObject gimmickObject) {}
}
