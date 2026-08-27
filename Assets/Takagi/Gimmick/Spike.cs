using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike:GimmickBase
{
    // トゲのサウンドのID
    private const int _SOUND_ID_SPIKE_ACTION = 0;
    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        // 当たった瞬間でなければ処理しない
        if (hitType != eHitType.Enter) return;
        Debug.Log("ギミック発動 : トゲ " + hitType);
        if (character == null) return;
        // キャラクターの死亡処理
        character.Dead();
        if (!character.IsPlayer()) return;

        // キャラクターがプレイヤーなら死亡サウンドを再生
        // 再生後の処理を渡す
        SoundManager.instance.PlaySE(_SOUND_ID_SPIKE_ACTION,Restart);
    }
    private void Restart()
    {
        // 同じシーンを再生する
        StageSceneManager.instance.LoadCurrentStage();

    }
}
