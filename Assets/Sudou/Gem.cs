using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Gem : GimmickBase
{
    /// <summary>
    /// 効果を発動したGemのオブジェクト情報
    /// </summary>
    struct GemObject
    {
        public GemReaction gemReaction;
        public GimmickObject gimmickObject;
    }
    /// <summary>
    /// 今までに効果を発動したGemのリスト
    /// </summary>
    private List<GemObject> _actionGemList = null;

    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        if (hitType == eHitType.Enter)
        {
            Debug.Log("ダッシュ回復");
            character.RechargeDash();
        }
    }
    public override void ToObjectAction(GimmickObject gimmickObject, eHitType hitType)
    {
        GemReaction(gimmickObject);
    }

    private void GemReaction(GimmickObject gimmickObj)
    {
        if(_actionGemList==null)_actionGemList=new List<GemObject>();

        // 引数のギミックがリスト内にあるかどうかチェック
        for (int i = 0; i < _actionGemList.Count; i++)
        {
            if (gimmickObj != _actionGemList[i].gimmickObject) continue;
            // 接触リアクションを実行
            _actionGemList[i].gemReaction.HitReaction();
            // 実行したのでreturn
            return;
        }
        //リスト内になければ

        // GemReactionを持っているかどうか取得
        GemReaction reaction=gimmickObj.GetComponent<GemReaction>();
        // GemReactionを持っていなければ処理しない
        if (reaction == null) return;

        GemObject gemObject=new GemObject();
        gemObject.gimmickObject=gimmickObj;
        gemObject.gemReaction = reaction;
        // リストに追加
        _actionGemList.Add(gemObject);

        // 接触リアクションを実行
        reaction.HitReaction();
    }

}
