using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D _coll;

    private void Awake()
    {
        _coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))      // ����� ���� ��� ���͸� �ѹ� �ɾ�� �� 
            return;

        // �÷��̾� ������ ��������
        Vector3 playerPos = GameManager._instance._player.transform.position;
        // �� ������ ��������
        Vector3 myPos = transform.position;

        //Vector3 playerDir = GameManager._instance._player._inputVec;  // �÷��̾ ��� �������� ������


        switch (transform.tag) 
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;    // �÷��̾� ��ġ���� Ÿ�ϸ� ��ġ ������� �Ÿ� ���밪���� ���ϱ�
                float diffY = playerPos.y - myPos.y;    
                float dirX = diffX < 0 ? -1 : 1;      // Normalized ������� �ʴ� ��� �� Input Manager ����ϸ� �̰� ��� ���ص� ������.
                float dirY = diffY < 0 ? -1 : 1;
                // �÷��̾�� Ÿ�ϸ��� �Ÿ��� ����ؼ� �ű�� ������ .
                diffX = Mathf.Abs(diffX); ;
                diffY = Mathf.Abs(diffY);
                if (diffX > diffY)   //�÷��̾ X�����θ� �־����ٴ� �̾߱��� �ᱹ Ÿ�ϸʵ� x������ �����Ѵٴ°���
                {
                    // 40�� ���� ������ �ᱹ ���� �ΰ��� ����Ű� �Ѱ��� 20�̴� �� ��40�� �ǳʶپ���Ѵ� �׷��� ���
                    transform.Translate(Vector3.right * dirX * 40);     
                }
                else if(diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                else
                {
                    // ���۽� �밢������ �̵��ϸ� Ÿ�ϸ� �Ѱ��� �̵��� ���� �ʴ´�. �װ� �ذ��ϱ� ���ؼ� �߰���
                    transform.Translate(dirX * 40, dirY * 40, 0);   
                }
                break;

            case "Enemy":
                // ���� ��� ���� �ݶ��̴��� �� �����̿��� �̷��� ������ �ɾ���.
                if(_coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    // �÷��̾� �տ� �����°� �� ���� �����ؼ� �̷��� �Ѵ�. �� �ϳ��� ũ�⸸ŭ �̵���Ŵ.
                    //transform.Translate(playerDir * 20  + new Vector3(Random.Range(3f, 3f), Random.Range(-3f, 3f), 0f));
                    // �÷��̾� �̵����� �ϴ°� �ּ�ó���ϴ� ���̰� ���� �ϴ� ���� �÷��̾�� Ÿ�ϸ� �Ÿ��� ����ϴ� ��
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
