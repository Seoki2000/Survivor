using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 장면이 한개여서 싱글톤 로직까지 사용하진 않는다. 
    public static GameManager _instance;        // 바로 메모리 사용
    [Header("Game Control")] 
    public bool _isLive; // 게임 일시정지를 위해서
    public float _gameTime;
    public float _maxGameT = 20;       // 테스트용 

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
        Application.targetFrameRate = 60;   // 프레임 지정 없으면 기본값 30임
    }
    public void GameStart(int id)
    {
        _playerId = id;
        _health = _maxHealth;

        _player.gameObject.SetActive(true);
        _uiLevelUp.Select(_playerId % 2);   // 시작 무기 지급
        _isLive = true;
        Resume();       // 이걸 해줘야 Stop함수를 사용해서 델타타임이 0인채로 재시작이 아닌 1로 재시작을 할 수 있음 

        AudioManager._instance.PlayBgm(true);
        AudioManager._instance.PlaySfx(AudioManager.Sfx.Selet);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoution());
    }

    IEnumerator GameOverRoution()
    {
        // 바로 실행하면 묘비가 보이기도 전에 게임 오버 화면이 나와서 코루틴을 이용해서 딜레이를 줌.
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
        // 바로 실행하면 묘비가 보이기도 전에 게임 오버 화면이 나와서 코루틴을 이용해서 딜레이를 줌.
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
        // 씬 이름으로 해도 상관없지만 일단 셋팅에 들어간게 한개만 있어서 이걸로 한다. 
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
        // 마지막에 레벨업 창이 나오지 않기 위해서 
        if(!_isLive) 
            return;

        _exp++;

        if(_exp == _nextExp[Mathf.Min(_level, _nextExp.Length - 1)])    // Min함수를 사용하여 최고 경험치를 그대로 사용하도록 변경
        {
            _level++;
            _exp = 0;
            _uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        _isLive = false;
        Time.timeScale = 0;     // 기본값은 1로 0으로 하면 멈추고 1로 하면 기본, 2로 하면 두배 이런식으로 사용한다.
        _uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        _isLive = true;
        Time.timeScale = 1;
        _uiJoy.localScale = Vector3.one;
    }
}
