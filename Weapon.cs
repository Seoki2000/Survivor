using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 무기 id, 프리펩 id, 데미지, 개수, 회전속도 변수 선언
    public int _id;
    public int _prefabId;
    public float _damage;
    public int _count;
    public float _speed;

    float _timer;

    Player _player;

    void Awake()
    {
        _player = GameManager._instance._player;
    }
    /*void Start()
    {
        Init();
    }*/
    void Update()
    {
        if (!GameManager._instance._isLive)
            return;

        // 무기 아이디 별로 로직을 작성할거임
        switch (_id)
        {
            case 0:
                transform.Rotate(Vector3.back * _speed * Time.deltaTime);   // forward로 하는 경우 Init에 있는 스피드가 -150이어야 한다 
                break;
            default:
                _timer += Time.deltaTime;

                // 이 경우 원거리 공격 실행을 하면 된다.
                if(_timer > _speed)
                {
                    Fire();
                    _timer = 0;
                }
                break;
        }

        // Test 

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        _damage = damage; //* Character.Damage;
        _count += count;

        if(_id == 0)
        {
            Batch();
        }
        // 웨폰 업그레이드도 경우 결국 생성, 업그레이드, 기어자체가 새로 생겼을 때, 기어 자체가 레벨업을 했을 때 총 4가지의 경우 사용됨
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);  
    }
    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data._itemId;
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        _id = data._itemId;
        _damage = data._baseDamage; // * Character.Damage; 이런식으로 추가해주면 된다.
        _count = data._baseCount;   // * Character.Count;

        // 물론 데이터에서 프리팹을 넣어 줄 수 있긴 하지만, 풀 매니저에도 정확하게 맞춰서 해야해서 오브젝트의 독립성을 위해서 이렇게 작성함.
        for(int index = 0; index < GameManager._instance._pool._prefabs.Length; index++)
        {
            if(data._projectile == GameManager._instance._pool._prefabs[index])
            {
                _prefabId= index;
                break;  
            }
        }


        //초기화 방식이 id에 따라 다르게 
        switch (_id)
        {
            case 0:
                _speed = 150 * Character.WeaponSpeed;  
                Batch();
                break;
            default:
                // 원거리니까 속도는 발사속도를 뜻함
                _speed= 0.3f * Character.WeaponRate;    
                break;

        }

        // hand set
        Hands hand = _player._hands[(int)data._itemType];
        hand._spriter.sprite = data._hand;
        hand.gameObject.SetActive(true);

        // 플레이어가 가진 모든 자식들한테 ApplyGear를 하라고 말하는 것 
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for(int index = 0; index < _count; index++)
        {
            // 부모를 바꾸기 위해서 Transform 가져옴 
            Transform bullet;
            // 내가 가지고 있는 오브젝트를 활용하고 부족한것을 풀링에서 가져오기.
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager._instance._pool.Get(_prefabId).transform;
                bullet.parent = transform;
            }

            // 초기화 먼저 하고 해야함
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / _count; // 각도 계산해주기
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);    // 이동방향은 월드를 기준으로 
            bullet.GetComponent<Bullet>().Init(_damage, -100, Vector3.zero);    // -100은 무한으로 관통한다. 
        }
    }

    void Fire()
    {
        // 가까이 적이 있는 경우, 즉 범위 안에 몬스터가 있는 경우를 뜻함.
        if (!_player._scanner._nearsetTarget)
        {
            // 필터 항상 해주기.
            return;
        }

        // 적을 향해서 바라보는 방향을 구해준다.
        Vector3 targetPos = _player._scanner._nearsetTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        // 위치 및 회전방향 결정
        Transform bullet = GameManager._instance._pool.Get(_prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(_damage, _count, dir);   // 근거리와 다르게 관통력, 방향을 넣어준다.

        AudioManager._instance.PlaySfx(AudioManager.Sfx.Range);

    }
}

