using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // ���� id, ������ id, ������, ����, ȸ���ӵ� ���� ����
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

        // ���� ���̵� ���� ������ �ۼ��Ұ���
        switch (_id)
        {
            case 0:
                transform.Rotate(Vector3.back * _speed * Time.deltaTime);   // forward�� �ϴ� ��� Init�� �ִ� ���ǵ尡 -150�̾�� �Ѵ� 
                break;
            default:
                _timer += Time.deltaTime;

                // �� ��� ���Ÿ� ���� ������ �ϸ� �ȴ�.
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
        // ���� ���׷��̵嵵 ��� �ᱹ ����, ���׷��̵�, �����ü�� ���� ������ ��, ��� ��ü�� �������� ���� �� �� 4������ ��� ����
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
        _damage = data._baseDamage; // * Character.Damage; �̷������� �߰����ָ� �ȴ�.
        _count = data._baseCount;   // * Character.Count;

        // ���� �����Ϳ��� �������� �־� �� �� �ֱ� ������, Ǯ �Ŵ������� ��Ȯ�ϰ� ���缭 �ؾ��ؼ� ������Ʈ�� �������� ���ؼ� �̷��� �ۼ���.
        for(int index = 0; index < GameManager._instance._pool._prefabs.Length; index++)
        {
            if(data._projectile == GameManager._instance._pool._prefabs[index])
            {
                _prefabId= index;
                break;  
            }
        }


        //�ʱ�ȭ ����� id�� ���� �ٸ��� 
        switch (_id)
        {
            case 0:
                _speed = 150 * Character.WeaponSpeed;  
                Batch();
                break;
            default:
                // ���Ÿ��ϱ� �ӵ��� �߻�ӵ��� ����
                _speed= 0.3f * Character.WeaponRate;    
                break;

        }

        // hand set
        Hands hand = _player._hands[(int)data._itemType];
        hand._spriter.sprite = data._hand;
        hand.gameObject.SetActive(true);

        // �÷��̾ ���� ��� �ڽĵ����� ApplyGear�� �϶�� ���ϴ� �� 
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for(int index = 0; index < _count; index++)
        {
            // �θ� �ٲٱ� ���ؼ� Transform ������ 
            Transform bullet;
            // ���� ������ �ִ� ������Ʈ�� Ȱ���ϰ� �����Ѱ��� Ǯ������ ��������.
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager._instance._pool.Get(_prefabId).transform;
                bullet.parent = transform;
            }

            // �ʱ�ȭ ���� �ϰ� �ؾ���
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / _count; // ���� ������ֱ�
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);    // �̵������� ���带 �������� 
            bullet.GetComponent<Bullet>().Init(_damage, -100, Vector3.zero);    // -100�� �������� �����Ѵ�. 
        }
    }

    void Fire()
    {
        // ������ ���� �ִ� ���, �� ���� �ȿ� ���Ͱ� �ִ� ��츦 ����.
        if (!_player._scanner._nearsetTarget)
        {
            // ���� �׻� ���ֱ�.
            return;
        }

        // ���� ���ؼ� �ٶ󺸴� ������ �����ش�.
        Vector3 targetPos = _player._scanner._nearsetTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        // ��ġ �� ȸ������ ����
        Transform bullet = GameManager._instance._pool.Get(_prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(_damage, _count, dir);   // �ٰŸ��� �ٸ��� �����, ������ �־��ش�.

        AudioManager._instance.PlaySfx(AudioManager.Sfx.Range);

    }
}

