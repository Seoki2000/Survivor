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
    public int _channels;  // ���� �ٹ������� �Ҹ��� ���� �� �ְ� ���ֱ�.
    AudioSource[] _sfxPlayers;
    int _channelIndex;

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Selet, Win } // ���Ƿ� ���� �־� �� �� ����
    private void Awake()
    {
        _instance = this;
        Init();
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.loop = true; 
        _bgmPlayer.volume = _bgmVolume;
        _bgmPlayer.clip = _bgmClip;
        _bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        _sfxPlayers = new AudioSource[_channels];

        for(int index = 0; index < _sfxPlayers.Length; index++)
        {
            _sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayers[index].playOnAwake = false;
            _sfxPlayers[index].bypassListenerEffects = true;    // ȿ������ ���������� �α� ���ؼ� ���� �߰���
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
        // �����ִ� �÷��̾ �����Ű�� ���ؼ�
        for(int index = 0; index < _sfxPlayers.Length; index++)
        {
            // ä�� ������ŭ ��ȸ�ϵ��� ä���ε��� ���� Ȱ��
            int loopIndex = (index + _channelIndex) % _sfxPlayers.Length;   // �Ѿ�� �ʰ� ����ֱ� ���ؼ�

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
