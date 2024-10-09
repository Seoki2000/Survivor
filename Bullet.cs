using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ������, ���� ���� �ΰ��� �ʿ�
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

        // ���Ÿ� ������ ���
        if(per >= 0)
        {
            _rigid.velocity = dir * 15f;    // �ӽð���
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �ƴϰų� ���� ������ ��� 
        // -1�� ��� �����ϴ� ���� �־ -100 ���� �ش�.
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
        // ��������� �ѱ��
        if (!collision.CompareTag("Area") || _per == -100)
        {
            return;
        }

        // ���Ÿ��� ��� �����ֱ� ���߿� �׳� �ڷ�ƾ ������ ������ �ص� ������ ������
        gameObject.SetActive(false);
    }

}
