using UnityEngine;

public class Gem : GimmickBase
{
    public override void GimmickAction(Player character, eHitType hitType)
    {
        if (hitType == eHitType.Enter)
        {
            Debug.Log("ダッシュ回復");
            character.RechargeDash();
        }
    }
}
