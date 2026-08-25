using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージ選択をするクラス
/// シングルトンで管理する
/// </summary>
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
    /// <summary>
    /// 指定IDのステージのシーンを読み込む
    /// </summary>
    /// <param name="stageID"></param>
    public void LoadStage(int stageID)
    {
        // IDが不正値なら処理しない
        if (stageID < 0 || stageID >= SceneName.SCENE_STAGE.Length)
        {
            return;
        }
        // 指定IDのシーン名を取得
        string sceneName= SceneName.SCENE_STAGE[stageID];
        // 取得した名前のシーン名を読み込む
        SceneManager.LoadScene(sceneName);
        return;
    }
    /// <summary>
    /// ステージ選択シーンに遷移する
    /// </summary>
    public void LoadStageSelect()
    {
        SceneManager.LoadScene(SceneName.SCENE_SELECTSCENE);
        return;
    }
    

}
