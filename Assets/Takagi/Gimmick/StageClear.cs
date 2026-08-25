using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClear : GimmickBase
{
    public override void OtherAction(eHitType hitType)
    {
        if (hitType != eHitType.Enter) return;
        StageSceneManager.instance.LoadNextStage();
    }
}
