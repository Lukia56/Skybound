using UnityEngine;

public class GimmickMath
{
    public static readonly float _TO_DEGREE = 180.0f / Mathf.PI;
    public static readonly float _TO_RADIAN = Mathf.PI / 180.0f;
    /// <summary>
    /// デグリー角からベクトルを取得する関数
    /// 0度の時は上に飛ぶ
    /// </summary>
    /// <param name="radian"></param>
    /// <returns></returns>
    public static Vector2 RadianToVec(float radian)
    {
        float x = -Mathf.Sin(radian);
        float y = Mathf.Cos(radian);
        Vector2 vec = new Vector2(x, y);
        return vec;
    }
}
