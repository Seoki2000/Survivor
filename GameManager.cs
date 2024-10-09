using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ����� �Ѱ����� �̱��� �������� ������� �ʴ´�. 
    public static GameManager _instance;        // �ٷ� �޸� ���
    [Header("Game Control")] 
    public bool _isLive; // ���� �Ͻ������� ���ؼ�
    public float _gameTime;
    public float _maxGameT = 20;       // �׽�Ʈ�� 

    [Header("Player Info")]
    public int _playerId;
    public float _health;
    public float _maxHealth = 100;
    public int _level;
    public int _kill;
    public int _exp;
    public int[] _nextExp = { 1, 3, 10, 100, 170, 230, 300, 400, 520, 650 };

    [Header("Game Object")]
    public PoolManager _pool;
    public Player _player;
    public LevelUp _uiLevelUp;
    public Result _uiResult;
    public GameObject _enemyClean;
    public Transform _uiJoy;

    void Awake()
    {
        _instance = this;
        Application.targetFrameRate = 60;   // ������ ���� ������ �⺻�� 30��
    }
    public void GameStart(int id)
    {
        _playerId = id;
        _health = _maxHealth;

        _player.gameObject.SetActive(true);
        _uiLevelUp.Select(_playerId % 2);   // ���� ���� ����
        _isLive = true;
        Resume();       // �̰� ����� Stop�Լ��� ����ؼ� ��ŸŸ���� 0��ä�� ������� �ƴ� 1�� ������� �� �� ���� 

        AudioManager._instance.PlayBgm(true);
        AudioManager._instance.PlaySfx(AudioManager.Sfx.Selet);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoution());
    }

    IEnumerator GameOverRoution()
    {
        // �ٷ� �����ϸ� ���� ���̱⵵ ���� ���� ���� ȭ���� ���ͼ� �ڷ�ƾ�� �̿��ؼ� �����̸� ��.
        _isLive = false;

        yield return new WaitForSeconds(0.5f);
        _uiResult.gameObject.SetActive(true);
        _uiResult.Lose();
        Stop();

        AudioManager._instance.PlayBgm(true);
        AudioManager._instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictroy()
    {
        StartCoroutine(GameVictoryRoution());
    }

    IEnumerator GameVictoryRoution()
    {
        // �ٷ� �����ϸ� ���� ���̱⵵ ���� ���� ���� ȭ���� ���ͼ� �ڷ�ƾ�� �̿��ؼ� �����̸� ��.
        _isLive = false;
        _enemyClean.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        _uiResult.gameObject.SetActive(true);
        _uiResult.Win();
        Stop();

        AudioManager._instance.PlayBgm(true);
        AudioManager._instance.PlaySfx(AudioManager.Sfx.Win);
    }
    public void Retry()
    {
        // �� �̸����� �ص� ��������� �ϴ� ���ÿ� ���� �Ѱ��� �־ �̰ɷ� �Ѵ�. 
        SceneManager.LoadScene(0);  
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (!_isLive)
            return;

        _gameTime += Time.deltaTime;

        if(_gameTime > _maxGameT)
        {
            _gameTime= _maxGameT;
            GameVictroy();
        }
    }

    public void GetExp()
    {
        // �������� ������ â�� ������ �ʱ� ���ؼ� 
        if(!_isLive) 
            return;

        _exp++;

        if(_exp == _nextExp[Mathf.Min(_level, _nextExp.Length - 1)])    // Min�Լ��� ����Ͽ� �ְ� ����ġ�� �״�� ����ϵ��� ����
        {
            _level++;
            _exp = 0;
            _uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        _isLive = false;
        Time.timeScale = 0;     // �⺻���� 1�� 0���� �ϸ� ���߰� 1�� �ϸ� �⺻, 2�� �ϸ� �ι� �̷������� ����Ѵ�.
        _uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        _isLive = true;
        Time.timeScale = 1;
        _uiJoy.localScale = Vector3.one;
    }
}
