using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    [Header("BGM")]
    public AudioClip _bgmClip;
    public float _bgmVolume;
    AudioSource _bgmPlayer;
    AudioHighPassFilter _bgmEffect;

    [Header("SFX")]
    public AudioClip[] _sfxClips;
    public float _sfxVolume;
    public int _channels;  // 동시 다발적으로 소리가 나올 수 있게 해주기.
    AudioSource[] _sfxPlayers;
    int _channelIndex;

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Selet, Win } // 임의로 숫자 넣어 줄 수 있음
    private void Awake()
    {
        _instance = this;
        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.loop = true; 
        _bgmPlayer.volume = _bgmVolume;
        _bgmPlayer.clip = _bgmClip;
        _bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        _sfxPlayers = new AudioSource[_channels];

        for(int index = 0; index < _sfxPlayers.Length; index++)
        {
            _sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayers[index].playOnAwake = false;
            _sfxPlayers[index].bypassListenerEffects = true;    // 효과음은 예외적으로 두기 위해서 따로 추가함
            _sfxPlayers[index].volume = _sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            _bgmPlayer.Play();
        }
        else
        {
            _bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        _bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        // 쉬고있는 플레이어에 실행시키기 위해서
        for(int index = 0; index < _sfxPlayers.Length; index++)
        {
            // 채널 개수만큼 순회하도록 채널인덱스 변수 활용
            int loopIndex = (index + _channelIndex) % _sfxPlayers.Length;   // 넘어가지 않게 잡아주기 위해서

            if (_sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            _channelIndex = loopIndex;
            _sfxPlayers[loopIndex].clip = _sfxClips[(int)sfx + ranIndex];
            _sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
