using System.Collections.Generic;
using UnityEngine;

public class Gem : GimmickBase
{

    public override void ToCharacterAction(CharacterBase character, eHitType hitType)
    {
        if (hitType == eHitType.Enter)
        {
            Debug.Log("ダッシュ回復");
            character.RechargeDash();
        }
    }

    // ======================
    // 以下は緊急で髙木が記述
    // ======================
    private const int _SOUND_ID_GEM_ACTION = 4;
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
    public override void ToObjectAction(GimmickObject gimmickObject, eHitType hitType)
    {
        if (hitType != eHitType.Enter) return;
        GemReaction(gimmickObject);
        SoundManager.instance.PlaySE(_SOUND_ID_GEM_ACTION);
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
