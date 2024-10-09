using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _speed;
    public float _health;
    public float _maxHealth;
    public RuntimeAnimatorController[] _animCon;

    public Rigidbody2D _target; // 따라가기 위해서
    Collider2D _coll;

    public bool _isLive;   // 테스트용으로 일단은 true로 해둔다.

    Rigidbody2D _rigid;
    Animator _anim;
    SpriteRenderer _sprite;
    WaitForFixedUpdate _wait;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!GameManager._instance._isLive)
            return;

        // 필터 넣어줘서 오류 막아주기. 죽으면 그 이후 실행 x 
        if (!_isLive || _anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))   // 레이어 번호를 넣어주는 것이다. 
            return;
        

        // 당연히 플레이어 방향으로 가야한다.
        Vector2 dirVec = _target.position - _rigid.position;
        Vector2 nextVec = dirVec.normalized * _speed * Time.fixedDeltaTime; // 방향키를 눌러서 이동하는 다음 위치의 값이다.

        // 플레이어의 키입력 값을 더한 이동 = 몬스터의 방향값을 더한 이동
        _rigid.MovePosition(_rigid.position + nextVec);
        _rigid.velocity = Vector2.zero; //물리 속도가 이동에 영향을 주지 않도록 속도 제거
    }

    void LateUpdate()
    {
        if (!GameManager._instance._isLive)
            return;

        // 필터 넣어줘서 오류 막아주기. 죽으면 그 이후 실행 x 
        if (!_isLive)
            return;

        // 타겟이 왼쪽에만 있는 경우 true를 넣어주면 된다. 
        _sprite.flipX = _target.position.x < _rigid.position.x;     
    }

    void OnEnable()
    {
        _target = GameManager._instance._player.GetComponent<Rigidbody2D>();
        _isLive = true;
        _coll.enabled = true;
        _rigid.simulated = true;
        _sprite.sortingOrder = 2;   
        _anim.SetBool("Dead", false);
        _health = _maxHealth;
    }

    // 레벨링에 따른 몬스터 직접 컨트롤 
    public void Init(SpawnData data)
    {
        _anim.runtimeAnimatorController = _animCon[data._spriteType];
        _speed = data._speed;
        _maxHealth = data._health;
        _health = data._health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 몬스터가 무기에 충돌을 할 때만 하기 위해서 필터 써두기
        if (!collision.CompareTag("Bullet") || !_isLive)    // !isLive 추가 안하면 아래 경험치가 두번 획득하는 경우도 생길 수 있어서 
        {
            return;
        }

        _health -= collision.GetComponent<Bullet>()._damage;
        StartCoroutine("KnockBack");

        // 남은 체력을 조건으로 피격과 사망으로 로직 나누기
        if(_health > 0)
        {
            // 애니메이션, 넉백 넣어주기
            _anim.SetTrigger("Hit");
            AudioManager._instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            _isLive = false;
            _coll.enabled = false;
            _rigid.simulated = false;
            _sprite.sortingOrder = 1;   // 스프라이트 렌더러에서 레이어 
            _anim.SetBool("Dead", true);
            GameManager._instance._kill++;
            GameManager._instance.GetExp();

            if(GameManager._instance._isLive)
            {
                AudioManager._instance.PlaySfx(AudioManager.Sfx.Dead);
            }
            
        }
    }

    IEnumerator KnockBack()
    {
        yield return _wait;  // 다음 하나의 물리 프레임까지 딜레이 

        //플레이어와 반대 방향으로 넉백 
        Vector3 playerPos = GameManager._instance._player.transform.position;
        Vector3 dirVec = transform.position - playerPos;

        _rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // 넉백 크기를 곱해준 것 (Impulse는 즉발)
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
