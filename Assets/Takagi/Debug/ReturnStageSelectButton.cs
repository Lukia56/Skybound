using UnityEngine;

public class ReturnStageSelectButton : MonoBehaviour
{
    public void ReturnSelectStage()
    {
        StageSelect.instance.LoadStageSelect();
    }
}
