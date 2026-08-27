using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager
{
    const string _AUDIORESOURCE_ORIGIN_PATH = "Prefabs/AudioResource";
    private static SoundManager _instance=null;
    public static SoundManager instance
    {
        get
        {
            if (_instance == null) _instance = new SoundManager();
            return _instance;
        }
    }
    private AudioResource _audioResourceOrigin=null;
    private AudioResource _audioResource= null;
 

    private SoundManager()
    {
        // AudioSourceのプレハブ取得
        if (_audioResourceOrigin == null)
        {
            _audioResourceOrigin = Resources.Load<AudioResource>(_AUDIORESOURCE_ORIGIN_PATH);
            if (_audioResourceOrigin == null) return;
        }
        // AudioResourceの生成
        if (_audioResource == null)
        {
            _audioResource=GameObject.Instantiate(_audioResourceOrigin);
            // 更新処理を渡す
            _audioResource.Init(Update);
            GameObject.DontDestroyOnLoad(_audioResource);
        }
    }
    /// <summary>
    /// サウンドの再生が終わったら処理の実行を行う
    /// </summary>
    private void Update()
    {
       
    }
   public void PlaySE(int soundID,System.Action action=null)
    {
        if (_audioResource == null) return;
        AudioClip audio=_audioResource.GetSound(soundID);
        if (audio == null) return;
        _audioResource.PlaySound(audio,action);
    }
   public void PlayBGM(int bgmID)
    {
        if (_audioResource == null) return;
        AudioClip audio = _audioResource.GetBGM(bgmID);
        if (audio == null) return;
        _audioResource.PlayBGM(audio);
    }
   
       
}
