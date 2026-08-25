using UnityEngine;

public class ReturnStageSelectButton : MonoBehaviour
{
    public void ReturnSelectStage()
    {
        StageSceneManager.instance.LoadStageSelect();
    }
}
