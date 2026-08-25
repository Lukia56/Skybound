using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// フィールド上に配置するギミックの管理を行う
/// </summary>
public class GimmickManager
{
    private static GimmickManager _instance=null;
    public static GimmickManager instance {  get
        {
            if (_instance == null) _instance = new GimmickManager();
            return _instance;
        }
    }

    private List<GimmickBase> gimmickList;
    private GimmickManager()
    {
        int gimmickMax = (int)eGimmick.Max;
        gimmickList = new List<GimmickBase>(gimmickMax);
    
    }
    public GimmickBase CreateGimmick(eGimmick gimmickType)
    {
        // ギミックの番号を取得
        int gimmickID = (int)gimmickType;
        // 指定ギミックの内、未使用のものがあれば使用状態にして返す
        if(gimmickList.Count>0)
        for (int i = 0; i < gimmickList.Count; i++)
        {
                return gimmickList[i]; 
        }
        // 未使用のものがなければ新しく生成して返す
        GimmickBase gimmick = null;
        // 指定したギミックを生成
        switch (gimmickType)
        {
            case eGimmick.Spike:
                gimmick = new Spike();
                break;
            case eGimmick.Spring:
                gimmick = new Spring();
                break;
            case eGimmick.Gem:
                gimmick = new Gem();
                break;
            default:
                break;
        }
        return gimmick;
    }
}
