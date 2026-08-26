using UnityEngine;

public class GoalMask : MonoBehaviour
{
    private SpriteMask _spriteMask = null;
    private SpriteRenderer _spriteRenderer = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteMask = GetComponent<SpriteMask>();
        if (_spriteMask == null || _spriteRenderer == null) return;
        _spriteMask.sprite=_spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
