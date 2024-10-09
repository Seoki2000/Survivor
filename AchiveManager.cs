using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] _lockChacacter;
    public GameObject[] _unLockChacacter;
    public GameObject _uiNotice;

    enum Achive { UnlockPotato, UnlockOnion }
    Achive[] _achives;
    WaitForSecondsRealtime _wait;

    void Awake()
    {
        _achives = (Achive[])Enum.GetValues(typeof(Achive));
        _wait = new WaitForSecondsRealtime(5);
        // 만약 업적이 있다면 데이터 저장해서 다음에 시작해도 해금이 된 상태로 플레이가 가능하게
        if (!PlayerPrefs.HasKey("MyData")) 
        {
            Init();
        }
    }

    void Init()
    {
        // 개인 핸드폰에 저장하는 것 
        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achive achive in _achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    private void Start()
    {
        UnlockCharacter();
    }
    void UnlockCharacter()
    {
        for(int index = 0; index < _lockChacacter.Length; index++)
        {
            string achiveName = _achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            _lockChacacter[index].SetActive(!isUnlock);     // 잠겨있는거니 부정으로 
            _unLockChacacter[index].SetActive(isUnlock);
        }
    }
    void LateUpdate()
    {
        foreach(Achive achive in _achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockPotato:
                isAchive = GameManager._instance._kill >= 10;
                break;
            case Achive.UnlockOnion:
                isAchive = GameManager._instance._gameTime == GameManager._instance._maxGameT;
                break;
        }

        // 조건을 달성한 경우
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) 
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int index = 0; index < _uiNotice.transform.childCount; index++)
            {
                bool isAct = index == (int)achive;
                _uiNotice.transform.GetChild(index).gameObject.SetActive(isAct);
            }

            StartCoroutine(NoticeRountin());
        }

    }

    IEnumerator NoticeRountin()
    {
        _uiNotice.SetActive(true);
        AudioManager._instance.PlaySfx(AudioManager.Sfx.Selet);
        yield return _wait;

        _uiNotice.SetActive(false);
    }
}
