using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect
{
    private static StageSelect _instance = null;

    public static StageSelect instance
    {
        get
        {
            if (_instance == null) _instance = new StageSelect();
            return _instance;
        }
    }

    public void LoadStage(int stageID)
    {
        if (stageID < 0 || stageID >= SceneName._SCENE_STAGE.Length)
        {
            return;
        }
        string sceneName= SceneName._SCENE_STAGE[stageID];
        SceneManager.LoadScene(sceneName);
        return;
    }
    public void LoadStageSelect()
    {
        SceneManager.LoadScene(SceneName._SCENE_SELECTSCENE);
        return;
    }
    

}
