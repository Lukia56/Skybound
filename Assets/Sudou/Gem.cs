using UnityEngine;

public class Gem : GimmickBase
{
    public override void ToPlayerAction(Player character, eHitType hitType)
    {
        if (hitType == eHitType.Enter)
        {
            Debug.Log("ダッシュ回復");
            character.RechargeDash();
        }
    }
}
