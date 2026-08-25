using UnityEngine;

public class Spike:GimmickBase
{
    public override void GimmickAction(Player character, eHitType hitType)
    {
        // 当たった瞬間でなければ処理しない
        if (hitType != eHitType.Enter) return;
        Debug.Log("トゲ : 効果発動");
        if (character == null) return;
        character.Dead();
    }
}
