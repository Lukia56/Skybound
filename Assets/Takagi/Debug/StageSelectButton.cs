using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public void SelectStage(int stageID)
    {
        Debug.Log("asdtga");
        StageSceneManager.instance.LoadStage(stageID);
    }
}
