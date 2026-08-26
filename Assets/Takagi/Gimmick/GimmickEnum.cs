
/// <summary>
/// どのように接触したか
/// </summary>
public enum eHitType
{
    Invalid=-1, // 不正値
    Enter,      // 当たった瞬間
    Stay,       // 当たっている間
    Exit,       // 離れた瞬間
    Max,
}

/// <summary>
/// ギミックの種類
/// </summary>
public enum eGimmick
{
    Invalid=-1,
    Spike,      // トゲ
    Spring,     // バネ
    Gem,        // ジェム
    Fall,       // 落下ブロック
    HideBlock,  // 崩れる足場
    StageClear,  // ゲームクリア
    Max,
}

/// <summary>
/// 4方向
/// </summary>
public enum eDirectionFour {
    Invalid=-1,
    Up,
    Right,
    Down,
    Left,
    Max,
}
