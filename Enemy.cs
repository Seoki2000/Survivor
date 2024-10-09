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

    public Rigidbody2D _target; // ���󰡱� ���ؼ�
    Collider2D _coll;

    public bool _isLive;   // �׽�Ʈ������ �ϴ��� true�� �صд�.

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

        // ���� �־��༭ ���� �����ֱ�. ������ �� ���� ���� x 
        if (!_isLive || _anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))   // ���̾� ��ȣ�� �־��ִ� ���̴�. 
            return;
        

        // �翬�� �÷��̾� �������� �����Ѵ�.
        Vector2 dirVec = _target.position - _rigid.position;
        Vector2 nextVec = dirVec.normalized * _speed * Time.fixedDeltaTime; // ����Ű�� ������ �̵��ϴ� ���� ��ġ�� ���̴�.

        // �÷��̾��� Ű�Է� ���� ���� �̵� = ������ ���Ⱚ�� ���� �̵�
        _rigid.MovePosition(_rigid.position + nextVec);
        _rigid.velocity = Vector2.zero; //���� �ӵ��� �̵��� ������ ���� �ʵ��� �ӵ� ����
    }

    void LateUpdate()
    {
        if (!GameManager._instance._isLive)
            return;

        // ���� �־��༭ ���� �����ֱ�. ������ �� ���� ���� x 
        if (!_isLive)
            return;

        // Ÿ���� ���ʿ��� �ִ� ��� true�� �־��ָ� �ȴ�. 
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

    // �������� ���� ���� ���� ��Ʈ�� 
    public void Init(SpawnData data)
    {
        _anim.runtimeAnimatorController = _animCon[data._spriteType];
        _speed = data._speed;
        _maxHealth = data._health;
        _health = data._health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ���Ͱ� ���⿡ �浹�� �� ���� �ϱ� ���ؼ� ���� ��α�
        if (!collision.CompareTag("Bullet") || !_isLive)    // !isLive �߰� ���ϸ� �Ʒ� ����ġ�� �ι� ȹ���ϴ� ��쵵 ���� �� �־ 
        {
            return;
        }

        _health -= collision.GetComponent<Bullet>()._damage;
        StartCoroutine("KnockBack");

        // ���� ü���� �������� �ǰݰ� ������� ���� ������
        if(_health > 0)
        {
            // �ִϸ��̼�, �˹� �־��ֱ�
            _anim.SetTrigger("Hit");
            AudioManager._instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            _isLive = false;
            _coll.enabled = false;
            _rigid.simulated = false;
            _sprite.sortingOrder = 1;   // ��������Ʈ ���������� ���̾� 
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
        yield return _wait;  // ���� �ϳ��� ���� �����ӱ��� ������ 

        //�÷��̾�� �ݴ� �������� �˹� 
        Vector3 playerPos = GameManager._instance._player.transform.position;
        Vector3 dirVec = transform.position - playerPos;

        _rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // �˹� ũ�⸦ ������ �� (Impulse�� ���)
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
