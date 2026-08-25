using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GimmickObject : MonoBehaviour
{
    [SerializeField] public GimmickBase gimmick;
    [SerializeField] private GimmickImage _image=null;
    [SerializeField] private eGimmick _gimmickType=eGimmick.Invalid;
    struct HitCharacter
    {
        public Player character;
        public GameObject Object;
    }
    [SerializeField] bool isHit = false;

    /// <summary>
    /// 当たったキャラクターの配列
    /// </summary>
    private List<HitCharacter> _hitCharacters=new List<HitCharacter>();
    /// <summary>
    /// 当たっているオブジェクト配列
    /// </summary>
    private List<GameObject> _hitStayObjects=new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gimmick == null)
        {
            gimmick = GimmickManager.instance.CreateGimmick(_gimmickType);
        }
    }
   
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
    public void StartEffect()
    {
        if (_image) _image.StartActionEffect();
    }
}
