using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 데미지, 관통 변수 두가지 필요
    public float _damage;
    public int _per;

    Rigidbody2D _rigid;
    void Awake()
    {
        _rigid= GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, int per, Vector3 dir)
    {
        _damage= damage;
        _per = per;

        // 원거리 무기인 경우
        if(per >= 0)
        {
            _rigid.velocity = dir * 15f;    // 임시값임
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 적이 아니거나 근접 무기인 경우 
        // -1인 경우 도달하는 일이 있어서 -100 값을 준다.
        if (!collision.CompareTag("Enemy") || _per == -100)
        {
            return;
        }

        _per--;
        if(_per < 0)
        {
            _rigid.velocity = Vector2.zero; 
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 근접무기면 넘기고
        if (!collision.CompareTag("Area") || _per == -100)
        {
            return;
        }

        // 원거리인 경우 없애주기 나중에 그냥 코루틴 돌려서 삭제를 해도 괜찮지 않을까
        gameObject.SetActive(false);
    }

}
