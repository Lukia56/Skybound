using UnityEngine;

public class Spike:GimmickBase
{
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        // 当たった瞬間でなければ処理しない
        if (hitType != eHitType.Enter) return;
        Debug.Log("ギミック発動 : トゲ " + hitType);
        if (character == null) return;
        character.Dead();
    }
}
