using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GimmickObject : MonoBehaviour
{
    /// <summary>
    /// 効果発動時に見た目上でのみ動くもの　(基本はnullで良い)
    /// </summary>
    [SerializeField] private GimmickImage _image=null;
    /// <summary>
    /// 自身のギミックの種類
    /// </summary>
    [SerializeField] private eGimmick _gimmickType=eGimmick.Invalid;
    /// <summary>
    /// 当たったキャラクターオブジェクトの情報
    /// </summary>
    struct HitCharacter
    {
        // キャラクターとしての情報
        public Player character;
        // オブジェクトとしての情報
        public GameObject Object;
    }

    /// <summary>
    /// 当たったキャラクターの配列
    /// </summary>
    private List<HitCharacter> _hitCharacters=new List<HitCharacter>();
    /// <summary>
    /// 当たっているオブジェクト配列
    /// </summary>
    private List<GameObject> _hitStayObjects=new List<GameObject>();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCharacterEnter(collision.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCharacterEnter(collision.gameObject);
    }
    /// <summary>
    /// オブジェクトが接触した瞬間の処理
    /// </summary>
    /// <param name="obj"></param>
    private void CheckCharacterEnter(GameObject obj)
    {
        // すでに配列にあればアクションを呼ぶ
        for (int i = 0; i < _hitCharacters.Count; i++){
            if (obj == _hitCharacters[i].Object){
                // キャラクターを取得
                Player player = _hitCharacters[i].character;
                // アクションの実行
                Action(player, eHitType.Enter);
                return;
            }
        }
        // オブジェクトがキャラクタークラスを所持しているかどうかを取得
        Player isPlayer = obj.GetComponent<Player>();
        if (isPlayer!=null){
            // 所持していれば効果発動
            Action(isPlayer, eHitType.Enter);
            // 配列に存在していなければ
            // 接触したキャラクター配列に追加
            HitCharacter hitCharacter;
            hitCharacter.character = isPlayer;
            hitCharacter.Object= obj;
            _hitCharacters.Add(hitCharacter);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        CheckCharacterExit(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckCharacterExit(collision.gameObject);
    }
    /// <summary>
    /// オブジェクトが離れた瞬間の処理
    /// </summary>
    /// <param name="obj"></param>
    private void CheckCharacterExit(GameObject obj)
    {
        // オブジェクトがキャラクタークラスを所持しているかどうかを取得
        Player isPlayer = obj.GetComponent<Player>();
        if (isPlayer!=null)
        {
            // 所持していれば効果発動
            Action(isPlayer, eHitType.Exit);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckCharacterStay(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCharacterStay(collision.gameObject);
    }
    /// <summary>
    /// オブジェクトと接触している間の処理
    /// </summary>
    /// <param name="obj"></param>
    private void CheckCharacterStay(GameObject obj)
    {
        // すでに接触しているキャラクターの中から同じオブジェクトを探す
        for (int i = 0; i < _hitCharacters.Count; i--)
        {
                // 同じオブジェクトがあるとき
            if (_hitCharacters[i].Object == obj)
            {
                // オブジェクトが持っているキャラクターに対してアクションを行う
                Action(_hitCharacters[i].character, eHitType.Stay);
                break;
            }
        }
    }
    private void Action(Player character,eHitType hitType)
    {
       GimmickManager.instance.Action(character,_gimmickType,this,hitType);
    }
    /// <summary>
    /// 自身のギミックが見た目上動く場合の動作開始処理
    /// </summary>
    public void StartEffect()
    {
        if (_image) _image.StartActionEffect();
    }
}
