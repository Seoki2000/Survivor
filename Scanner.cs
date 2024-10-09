using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    // 범위, 레이어, 스캔 결과 배열, 가장 가까운 목표를 담을 변수 선언
    public float _scanRange;
    public LayerMask _targetLay;
    public RaycastHit2D[] _targets;
    public Transform _nearsetTarget;

    private void FixedUpdate()
    {
        // 원형 형태로 검색하는 것 (zero로 하는 것은 원형으로 검색하고 ray를 쏘는 것이 아니여서 0을 넣었음
        // 캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이를 적어야한다.
        _targets = Physics2D.CircleCastAll(transform.position, _scanRange, Vector2.zero, 0, _targetLay);
        _nearsetTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100; // 큰거리 임시로 넣어두기

        foreach(RaycastHit2D target in _targets)
        {
            // 플레이어 위치부터
            Vector3 myPos = transform.position;
            // 타겟 위치 가져오기
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if(curDiff < diff)
            {
                // 계속 찾으면서 줄어드는걸로
                diff = curDiff;
                result = target.transform;
            }
        }

        // 결국 여기까지 오면 젤 가까운 타겟의 포지션을 가지고 있는다
        return result;
    }
}
