using UnityEngine;

public class Gem : GimmickBase
{
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        if (hitType == eHitType.Enter)
        {
            Debug.Log("ダッシュ回復");
            character.RechargeDash();
        }
    }
}
