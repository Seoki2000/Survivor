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
            // ������ ���� �ð� ����
            _timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        // ������ ���� ��ȯ
        GameObject enemy =  GameManager._instance._pool.Get(0);     // Enemy �� �������� �������� �Ŀ� ����
        enemy.transform.position = _spawnPoint[Random.Range(1, _spawnPoint.Length)].position;   // GetComponentsInChildern�� �ڱ� �ڽŵ� �����ͼ� 1���� �־������
        enemy.GetComponent<Enemy>().Init(_spawnData[_level]);
    }
}


// ����ȭ ���ֱ�

[System.Serializable]
public class SpawnData
{
    // �߰��� �Ӽ���. ��������Ʈ Ÿ�� ��ȯ �ð� ü�� �ӵ� 
    public int _spriteType;
    public float _spawnTime;
    public int _health;
    public float _speed;


}