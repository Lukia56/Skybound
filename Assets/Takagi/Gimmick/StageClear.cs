using UnityEngine.InputSystem;

public class StageClear : GimmickBase
{
    private const int _SOUND_ID_STAGECLEAR = 1;
    /// <summary>
    /// キャラクターに対する処理
    /// </summary>
    /// <param name="character"></param>
    /// <param name="hitType"></param>
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        if (hitType != eHitType.Enter) return;
        // プレイヤーでなければ処理しない
        if (!character.IsPlayer()) return;

        // プレイヤーの入力操作を受け付けないようにする
        character.OnReachGoal();

        // UIの入力をオフにする
        InputSystem.actions.FindActionMap("UI").Disable();
    }
    /// <summary>
    /// その他の処理
    /// </summary>
    /// <param name="hitType"></param>
    public override void OtherAction(eHitType hitType)
    {
        // 接触した瞬間のみ処理する
        if (hitType != eHitType.Enter) return;
        // ステージクリアのサウンド再生 : ステージクリア処理を引数として渡す
        SoundManager.instance.PlaySE(_SOUND_ID_STAGECLEAR, Clear);
    }
    /// <summary>
    /// ステージクリア処理
    /// </summary>
    public void Clear()
    {
        // UIの入力をオンにする
        InputSystem.actions.FindActionMap("UI").Enable();

        // シーン遷移
        StageSceneManager.instance.LoadNextStage();
    }
}
