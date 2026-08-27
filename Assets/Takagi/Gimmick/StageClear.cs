using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClear : GimmickBase
{
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        if (hitType != eHitType.Enter) return;
        // プレイヤーの入力操作を受け付けないようにする
        character.OnReachGoal();
    }
    public override void OtherAction(eHitType hitType)
    {
        if (hitType != eHitType.Enter) return;
        SoundManager.instance.PlaySE(1, Clear);
    }
    public void Clear()
    {
        StageSceneManager.instance.LoadNextStage();
    }
}
