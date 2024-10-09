using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour 
{
    public Vector2 _inputVec; // 움직임
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
        _hands = GetComponentsInChildren<Hands>(true);      // true를 넣어주면 setactvie를 true로 활성화해준다.
    }

    void OnEnable()
    {
        // 시작 캐릭터 효과 추가
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
        /*        // 힘을 주는 경우
                rigid.AddForce(inputVec);

                // 속도 제어
                rigid.velocity = inputVec;*/

        if (!GameManager._instance._isLive)
            return;

        // 위치 이동
        Vector2 nextVec = _inputVec.normalized * _speed * Time.fixedDeltaTime;     // Player Input 사용하면 normalized 제거
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

        // 사망 로직
        if(GameManager._instance._health <= 0)
        {
            // 묘비 형태로 바뀌기 때문에 Shadow, Area 이후 전부 false를 해줘야한다. 그래서 index는 2부터 시작.
            for(int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager._instance.GameOver();

        }
    }

    // 플레이어 input 넣어준 경우 
    void OnMove(InputValue val)
    {
        _inputVec = val.Get<Vector2>();
    }
}
