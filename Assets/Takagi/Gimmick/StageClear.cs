using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClear : GimmickBase
{
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
