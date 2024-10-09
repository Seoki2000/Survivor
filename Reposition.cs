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
        if (!collision.CompareTag("Area"))      // 벗어나지 않은 경우 필터를 한번 걸어둔 것 
            return;

        // 플레이어 포지션 가져오기
        Vector3 playerPos = GameManager._instance._player.transform.position;
        // 내 포지션 가져오기
        Vector3 myPos = transform.position;

        //Vector3 playerDir = GameManager._instance._player._inputVec;  // 플레이어가 어느 방향으로 가는지


        switch (transform.tag) 
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;    // 플레이어 위치에서 타일맵 위치 계산으로 거리 절대값으로 구하기
                float diffY = playerPos.y - myPos.y;    
                float dirX = diffX < 0 ? -1 : 1;      // Normalized 사용하지 않는 경우 즉 Input Manager 사용하면 이거 사용 안해도 괜찮다.
                float dirY = diffY < 0 ? -1 : 1;
                // 플레이어와 타일맵의 거리를 계산해서 옮기는 것으로 .
                diffX = Mathf.Abs(diffX); ;
                diffY = Mathf.Abs(diffY);
                if (diffX > diffY)   //플레이어가 X축으로만 멀어졌다는 이야기임 결국 타일맵도 x축으로 가야한다는거임
                {
                    // 40이 나온 이유는 결국 맵을 두개로 만들거고 한개가 20이니 즉 총40을 건너뛰어야한다 그래서 사용
                    transform.Translate(Vector3.right * dirX * 40);     
                }
                else if(diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                else
                {
                    // 시작시 대각선으로 이동하면 타일맵 한개가 이동을 하지 않는다. 그걸 해결하기 위해서 추가함
                    transform.Translate(dirX * 40, dirY * 40, 0);   
                }
                break;

            case "Enemy":
                // 죽을 경우 먼저 콜라이더를 끌 생각이여서 이렇게 조건을 걸었다.
                if(_coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    // 플레이어 앞에 나오는게 더 좋다 생각해서 이렇게 한다. 맵 하나의 크기만큼 이동시킴.
                    //transform.Translate(playerDir * 20  + new Vector3(Random.Range(3f, 3f), Random.Range(-3f, 3f), 0f));
                    // 플레이어 이동으로 하는게 주석처리하는 것이고 지금 하는 것이 플레이어와 타일맵 거리를 계산하는 것
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
