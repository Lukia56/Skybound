using UnityEngine;

public class Gem : GimmickBase
{
    public override void GimmickAction(Player character, eHitType hitType)
    {
        character.RechargeDash();
    }
}
