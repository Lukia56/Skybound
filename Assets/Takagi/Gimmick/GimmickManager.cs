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
    public class GimmickData
    {
        public GimmickData(GimmickBase gimmick) { this.gimmick = gimmick; }
        public bool _isUse = false;
        public GimmickBase gimmick { get; private set; } = null;
    }

    private List<List<GimmickData>> gimmickList;
    private GimmickManager()
    {
        int gimmickMax = (int)eGimmick.Max;
        gimmickList = new List<List<GimmickData>>();
        for (int i = 0; i < gimmickMax; i++)
        {
            gimmickList.Add(new List<GimmickData>()); 

        }
    }
    public GimmickBase CreateGimmick(eGimmick gimmickType)
    {
        // ギミックの番号を取得
        int gimmickID = (int)gimmickType;
        // 指定ギミックの内、未使用のものがあれば使用状態にして返す
        if(gimmickList.Count>0)
        for (int i = 0; i < gimmickList[gimmickID].Count; i++)
        {
            GimmickData gimmickData = gimmickList[gimmickID][i];
            if (gimmickData._isUse==true) continue;
            gimmickData._isUse = true;
            return gimmickData.gimmick;
        }
        // 未使用のものがなければ新しく生成して返す
        GimmickBase gimmick = null;
        // 指定したギミックを生成
        switch (gimmickType)
        {
            case eGimmick.Spike:
                gimmick = new Spike();
                break;
            case eGimmick.Gem:
                gimmick = new Gem();
                break;
            default:
                break;
        }
        // 生成できていれば使用状態にして配列に加える
        if (gimmick != null)
        {
            GimmickData data = new GimmickData(gimmick);
            data._isUse = true;
            gimmickList[gimmickID].Add(data);
        }
        return gimmick;
    }
    public void UnUseGimmick(GimmickBase gimmick)
    {

        for (int i = 0; i < gimmickList.Count; i++)
            for (int j = 0; j < gimmickList[i].Count; j++)
            {
                if (gimmickList[i][j].gimmick != gimmick) continue;
                gimmickList[i][j]._isUse = false;
            }
    }

}
