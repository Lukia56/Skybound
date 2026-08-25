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
        for(int i = 0; i < gimmickMax; i++) {
            // 指定したギミックを生成
            switch ((eGimmick)i)
            {
                case eGimmick.Spike:
                    gimmickList.Add(new Spike());
                    break;
                case eGimmick.Spring:
                    gimmickList.Add(new Spring());
                    break;
                case eGimmick.Gem:
                    gimmickList.Add(new Gem());
                    break;
                default:
                    break;
            }
        }
    }
    public GimmickBase CreateGimmick(eGimmick gimmickType)
    {
        // ギミックの番号を取得
        int gimmickID = (int)gimmickType;
        // 指定ギミックの内、未使用のものがあれば使用状態にして返す
        if (gimmickID < 0 || gimmickID >= gimmickList.Count)
        {
            return null;
        }
        return gimmickList[gimmickID];
    }
    public void Action(Player character,eGimmick gimmickType,GimmickObject gimmickObj, eHitType hitType)
    {
        Debug.Log("効果発動" + hitType);
        int gimmickID = (int)gimmickType;
        GimmickBase gimmick = gimmickList[gimmickID];
        gimmick.SetGimmickObject(gimmickObj);
        gimmick.ToPlayerAction(character, hitType);
        gimmick.ToObjectAction(gimmickObj);
    }
}
