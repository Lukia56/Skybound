using TMPro;
using UnityEngine;

/// <summary>
/// 点滅するテキスト
/// </summary>
public class TextFlash : MonoBehaviour
{
    [SerializeField]
    private float _switchDuration = 0.0f;

    private float _timer = 0.0f;

    private TextMeshProUGUI _text = null;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_timer > 0.0f)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _text.enabled = !_text.enabled;

            _timer = _switchDuration;
        }
    }
}
