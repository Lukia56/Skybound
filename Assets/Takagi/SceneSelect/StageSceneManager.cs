using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージ選択をするクラス
/// シングルトンで管理する
/// </summary>
public class StageSceneManager
{
    private static StageSceneManager _instance = null;
    public static StageSceneManager instance
    {
        get
        {
            if (_instance == null) _instance = new StageSceneManager();
            return _instance;
        }
    }
    /// <summary>
    /// 現在のステージID
    /// </summary>
    public int currentStageID { get; private set; } = -1;
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
        // 現在のステージIDを更新
        currentStageID = stageID;
        // 指定IDのシーン名を取得
        string sceneName= SceneName.SCENE_STAGE[stageID];
        // 取得した名前のシーン名を読み込む
        SceneManager.LoadScene(sceneName);
        return;
    }
    /// <summary>
    /// 次のステージへ遷移
    /// </summary>
    public void LoadNextStage()
    {
        // 次のステージIDを求める
        int nextStageID = currentStageID + 1;
        // 次のステージIDが不正値での時ステージ選択シーンへ遷移
        if (nextStageID < 0 ||
            nextStageID >= SceneName.SCENE_STAGE.Length)
        {
            LoadTitleScene();
        }
        // 現在のステージIDを更新
        currentStageID = nextStageID;
        // ステージを読み込む
        LoadStage(currentStageID);
    }
    /// <summary>
    /// 1つ前のステージへ遷移
    /// </summary>
    public void LoadPrevStage()
    {
        // 1つ前のステージIDを求める
        int nextStageID = currentStageID - 1;
        // 次のステージIDが不正値での時ステージ選択シーンへ遷移
        if (nextStageID < 0 ||
            nextStageID >= SceneName.SCENE_STAGE.Length)
        {
            LoadTitleScene();
        }
        // 現在のステージIDを更新
        currentStageID = nextStageID;
        // ステージを読み込む
        LoadStage(currentStageID);
    }
    /// <summary>
    /// 同じステージへ遷移
    /// </summary>
    public void LoadCurrentStage()
    {
        // 次のステージIDが不正値での時ステージ選択シーンへ遷移
        if (currentStageID < 0 ||
            currentStageID >= SceneName.SCENE_STAGE.Length)
        {
            LoadTitleScene();
        }
        // ステージを読み込む
        LoadStage(currentStageID);
    }
    /// <summary>
    /// ステージ選択シーンに遷移する
    /// </summary>
    public void LoadStageSelect()
    {
        SceneManager.LoadScene(SceneName.SCENE_SELECTSCENE);
        return;
    }
    /// <summary>
    /// タイトルシーンに遷移
    /// </summary>
    public void LoadTitleScene()
    {
        SceneManager.LoadScene(SceneName.SCENE_TITLE);
        return;
    }
}
