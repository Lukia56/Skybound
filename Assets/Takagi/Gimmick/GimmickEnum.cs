
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

public enum eGimmick
{
    Invalid=-1,
    Spike,      // トゲ
    Spring,     // バネ
    Max,
}
