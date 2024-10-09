using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour 
{
    public Vector2 _inputVec; // ������
    public float _speed;
    public Scanner _scanner;
    public Hands[] _hands;
    public RuntimeAnimatorController[] _animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        _scanner = GetComponent<Scanner>();
        _hands = GetComponentsInChildren<Hands>(true);      // true�� �־��ָ� setactvie�� true�� Ȱ��ȭ���ش�.
    }

    void OnEnable()
    {
        // ���� ĳ���� ȿ�� �߰�
        _speed *= Character.Speed;
        anim.runtimeAnimatorController = _animCon[GameManager._instance._playerId];
    }
    void Update()
    {
        if (!GameManager._instance._isLive)
            return;

        /*_inputVec.x = Input.GetAxisRaw("Horizontal");
        _inputVec.y = Input.GetAxisRaw("Vertical");*/
    }

    void FixedUpdate()
    {
        /*        // ���� �ִ� ���
                rigid.AddForce(inputVec);

                // �ӵ� ����
                rigid.velocity = inputVec;*/

        if (!GameManager._instance._isLive)
            return;

        // ��ġ �̵�
        Vector2 nextVec = _inputVec.normalized * _speed * Time.fixedDeltaTime;     // Player Input ����ϸ� normalized ����
        rigid.MovePosition(rigid.position + nextVec);     
    }

    void LateUpdate()
    {
        if (!GameManager._instance._isLive)
            return;

        anim.SetFloat("Speed", _inputVec.magnitude);

        if (_inputVec.x != 0)
        {
            spriter.flipX = _inputVec.x < 0;
        }

    }

    /*void OnMove(InputValue val)
    {
        _inputVec = val.Get<Vector2>();
    }*/

    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager._instance._isLive)
            return;

        GameManager._instance._health -= Time.deltaTime * 10;   

        // ��� ����
        if(GameManager._instance._health <= 0)
        {
            // ���� ���·� �ٲ�� ������ Shadow, Area ���� ���� false�� ������Ѵ�. �׷��� index�� 2���� ����.
            for(int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager._instance.GameOver();

        }
    }

    // �÷��̾� input �־��� ��� 
    void OnMove(InputValue val)
    {
        _inputVec = val.Get<Vector2>();
    }
}
