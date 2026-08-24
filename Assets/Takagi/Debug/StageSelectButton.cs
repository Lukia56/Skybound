using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public void SelectStage(int stageID)
    {
        Debug.Log("asdtga");
        StageSelect.instance.LoadStage(stageID);
    }
}
