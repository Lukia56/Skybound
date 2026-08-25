using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GimmickObject : MonoBehaviour
{
    [SerializeField] public GimmickBase gimmick;
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
    [SerializeField]int objCount=0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gimmick == null)
        {
            gimmick = GimmickManager.instance.CreateGimmick(_gimmickType);
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateHitObjects();
        objCount=_hitCharacters.Count;
    }

    private void UpdateHitObjects()
    {
        for(int i = _hitCharacters.Count-1; i >=0 ; i--)
        {
            if(_hitStayObjects.Exists(objData => objData == _hitCharacters[i].Object))
            {
                Action(_hitCharacters[i].character, eHitType.Stay);
                break;
            }
            _hitCharacters.RemoveAt(i);
        }
        _hitStayObjects.Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckPlayer(collision.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckPlayer(collision.gameObject);
    }
    private void CheckPlayer(GameObject obj)
    {
        // オブジェクトがキャラクタークラスを所持しているかどうかを取得
        Player isPlayer = obj.GetComponent<Player>();
        if (isPlayer!=null)
        {
            // 所持していれば効果発動
            Action(isPlayer, eHitType.Enter);
            // 接触したキャラクター配列に追加
            HitCharacter hitCharacter;
            hitCharacter.character = isPlayer;
            hitCharacter.Object= obj;
            _hitCharacters.Add(hitCharacter);
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject hitObject = collision.gameObject;
        // オブジェクトがキャラクタークラスを所持しているかどうかを取得
        Player isPlayer = hitObject.GetComponent<Player>();
        if (isPlayer!=null)
        {
            // 所持していれば効果発動
            Action(isPlayer, eHitType.Enter);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 接触したオブジェクトを追加
        GameObject hitObject = collision.gameObject;
        _hitStayObjects.Add(hitObject);
        isHit = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 接触したオブジェクトを追加
        GameObject hitObject = collision.gameObject;
        _hitStayObjects.Add(hitObject);
        isHit = true;
    }
    private void Action(Player character,eHitType hitType)
    {
        Debug.Log("効果発動");
        gimmick.GimmickAction(character, hitType);
    }
}
