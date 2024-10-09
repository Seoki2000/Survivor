using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] _spawnPoint;
    public SpawnData[] _spawnData;
    public float _levelTime;

    int _level;
    float _timer;

    void Awake()
    {
        _timer = 0;
        _spawnPoint = GetComponentsInChildren<Transform>();
        _levelTime = GameManager._instance._maxGameT / _spawnData.Length;
    }
    void Update()
    {
        if (!GameManager._instance._isLive)
            return;

        _timer += Time.deltaTime;
        _level = Mathf.Min(Mathf.FloorToInt(GameManager._instance._gameTime / _levelTime), _spawnData.Length - 1);

        if(_timer > _spawnData[_level]._spawnTime)
        {
            // 레벨에 따라서 시간 조정
            _timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        // 레벨에 따라서 소환
        GameObject enemy =  GameManager._instance._pool.Get(0);     // Enemy 만 가져오고 나머지는 후에 조정
        enemy.transform.position = _spawnPoint[Random.Range(1, _spawnPoint.Length)].position;   // GetComponentsInChildern은 자기 자신도 가져와서 1부터 넣어줘야함
        enemy.GetComponent<Enemy>().Init(_spawnData[_level]);
    }
}


// 직렬화 해주기

[System.Serializable]
public class SpawnData
{
    // 추가할 속성들. 스프라이트 타입 소환 시간 체력 속도 
    public int _spriteType;
    public float _spawnTime;
    public int _health;
    public float _speed;


}