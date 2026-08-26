using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike:GimmickBase
{
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        // 当たった瞬間でなければ処理しない
        if (hitType != eHitType.Enter) return;
        Debug.Log("ギミック発動 : トゲ " + hitType);
        if (character == null) return;
        character.Dead();
        if (!character.IsPlayer()) return;
        SoundManager.instance.PlaySE(0,Restart);
    }
    public override void OtherAction(eHitType hitType)
    {
        if (hitType != eHitType.Enter) return;

    }
    private void Restart()
    {
        StageSceneManager.instance.LoadCurrentStage();

    }
}
