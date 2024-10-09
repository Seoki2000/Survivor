using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    // ����, ���̾�, ��ĵ ��� �迭, ���� ����� ��ǥ�� ���� ���� ����
    public float _scanRange;
    public LayerMask _targetLay;
    public RaycastHit2D[] _targets;
    public Transform _nearsetTarget;

    private void FixedUpdate()
    {
        // ���� ���·� �˻��ϴ� �� (zero�� �ϴ� ���� �������� �˻��ϰ� ray�� ��� ���� �ƴϿ��� 0�� �־���
        // ĳ���� ���� ��ġ, ���� ������, ĳ���� ����, ĳ���� ����, ��� ���̸� ������Ѵ�.
        _targets = Physics2D.CircleCastAll(transform.position, _scanRange, Vector2.zero, 0, _targetLay);
        _nearsetTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100; // ū�Ÿ� �ӽ÷� �־�α�

        foreach(RaycastHit2D target in _targets)
        {
            // �÷��̾� ��ġ����
            Vector3 myPos = transform.position;
            // Ÿ�� ��ġ ��������
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if(curDiff < diff)
            {
                // ��� ã���鼭 �پ��°ɷ�
                diff = curDiff;
                result = target.transform;
            }
        }

        // �ᱹ ������� ���� �� ����� Ÿ���� �������� ������ �ִ´�
        return result;
    }
}
