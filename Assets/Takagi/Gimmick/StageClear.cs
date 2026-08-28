using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StageClear : GimmickBase
{
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        if (hitType != eHitType.Enter) return;
        // プレイヤーの入力操作を受け付けないようにする
        character.OnReachGoal();

        // UIの入力をオフにする
        InputSystem.actions.FindActionMap("UI").Disable();
    }
    public override void OtherAction(eHitType hitType)
    {
        if (hitType != eHitType.Enter) return;
        SoundManager.instance.PlaySE(1, Clear);
    }
    public void Clear()
    {
        // UIの入力をオンにする
        InputSystem.actions.FindActionMap("UI").Enable();

        // シーン遷移
        StageSceneManager.instance.LoadNextStage();
    }
}
