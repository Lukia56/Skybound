using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class AudioResource : MonoBehaviour
{
    [SerializeField]private AudioClip[] _soundList =null; 
    [SerializeField]private AudioClip[] _bgmList =null;
    private List<SoundEvent> _soundEvent = null;
    private AudioSource _bgmSource = null;
    private const int _AUDIOSOURCE_INIT_NUM = 5;
    class SoundEvent
    {
        public AudioSource source;
        public System.Action actionEvent;
        public bool isPlaying;
    }
    private System.Action _updateAction = null;
    private void Start()
    {
    }
    public void Init(System.Action updateAction)
    {
        _updateAction = updateAction;
        _soundEvent = new List<SoundEvent>(_AUDIOSOURCE_INIT_NUM);
        for (int i = 0; i < _AUDIOSOURCE_INIT_NUM; i++)
        {
            // サウンドの配列を生成
            SoundEvent audioEvent = new SoundEvent();
            audioEvent.actionEvent = null;
            audioEvent.isPlaying = false;
            audioEvent.source = gameObject.AddComponent<AudioSource>();
            _soundEvent.Add(audioEvent);

            // BGMの配列を生成

        }
        // BGMのAudioSource生成
        AudioSource soundSource = gameObject.AddComponent<AudioSource>();
        // BGMをループさせる
        soundSource.loop = true;
        _bgmSource = soundSource;
    }
    private void Update()
    {
        for (int i = 0; i < _soundEvent.Count; i++)
        {
            SoundEvent audioEvent = _soundEvent[i];
            // 前回の再生状態の更新
            bool prevPlay = audioEvent.isPlaying;
            // 今回の再生状態の更新
            audioEvent.isPlaying = audioEvent.source.isPlaying;
            // 再生し終わった瞬間で、かつ再生終了時の処理が渡されていたら処理を行う
            if (prevPlay &&!audioEvent.isPlaying &&
                    audioEvent.actionEvent != null)
            {
                audioEvent.actionEvent();
            }
            _soundEvent[i].isPlaying = _soundEvent[i].source.isPlaying;
        }
    }
    public AudioClip GetSound(int soundID)
    {
        if(soundID<0||soundID>=_soundList.Length)return null;
        return _soundList[soundID];
    }
    public AudioClip GetBGM(int bgmID)
    {
        if(bgmID<0||bgmID>=_bgmList.Length)return null;
        return _bgmList[bgmID];
    }
    public void PlaySound(AudioClip clip, System.Action action)
    {
        if (clip == null) return;
        // 未使用のAudioSourceがあればそれを使う
        for (int i = 0; i < _soundEvent.Count; i++)
        {
            // 使用中ならスキップ
            if (_soundEvent[i].source.isPlaying) continue;
            _soundEvent[i].isPlaying=true;
            // 再生終了後の処理を設定
            _soundEvent[i].actionEvent=action;
            // クリップを設定
            _soundEvent[i].source.clip = clip;
            // 設定したクリップを再生
            _soundEvent[i].source.Play();
            // 再生できたのでreturn
            return;
        } // すべて使用中で再生できなかった時
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        // クリップを設定
        audioSource.clip = clip;
        // クリップを再生
        audioSource.Play();
        // 構造体の生成
        SoundEvent audioEvent = new SoundEvent();
        audioEvent.source = audioSource;
        audioEvent.isPlaying = false;
        audioEvent.actionEvent = action;
        // 生成したAudioSourceを配列に追加
        _soundEvent.Add(audioEvent);

    }
    public void PlayBGM(AudioClip clip)
    {
        // BGMを停止
        _bgmSource.Stop();
        // クリップを更新
        _bgmSource.clip = clip;
        // BGM再生
        _bgmSource.Play();
    }
}
