using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GimmickAnimation : MonoBehaviour
{
    private SpriteRenderer _spriteRemderer=null;
    // アニメーション画像リスト
    private List<Sprite> _sprites = null;
    // リスト初期化時の要素数
    private const int _ANIM_INIT_NUM = 5;
    [SerializeField] private int _loadStartIndex = 0;
    [SerializeField] private string _animPath = string.Empty;
    [SerializeField] private float _spriteChangeTime = 0.2f;
    [SerializeField] private float _spriteChangeCount = 0.0f;
    private int _currentSpriteIndex=0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRemderer=GetComponent<SpriteRenderer>();
        _sprites=new List<Sprite>(_ANIM_INIT_NUM);
        int spriteIndex = _loadStartIndex;
        while(true)
        {
            // インデックスを使いパスを取得
            string spritePath = _animPath + spriteIndex;
            // スプライトを取得
            Sprite loadSprite = Resources.Load<Sprite>(spritePath);
            // 読み込みができていなければループを抜ける
            if (loadSprite == null) break;
            // スプライトのリストに追加
            _sprites.Add(loadSprite);
            // インデックスを加算
            spriteIndex++;
        }
        _spriteChangeCount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime=Time.deltaTime;
        _spriteChangeCount += deltaTime;
        if (_spriteChangeCount < _spriteChangeTime) return;
        _currentSpriteIndex++;
        _spriteChangeCount = 0.0f;
        if (_currentSpriteIndex >= _sprites.Count) _currentSpriteIndex = 0;
        _spriteRemderer.sprite= _sprites[_currentSpriteIndex];
    }
}
