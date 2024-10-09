using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // �����յ��� ������ ����
    public GameObject[] _prefabs;

    // Ǯ ����� �ϴ� ����Ʈ�� 

    List<GameObject>[] _pools;


    private void Awake()
    {
        _pools = new List<GameObject>[_prefabs.Length];
        // ����Ʈ�� ��ȸ�ϸ鼭 �ʱ�ȭ 
        for (int index = 0; index < _pools.Length; index++)
        {
            _pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // ������ Ǯ�� ��� �ִ� ���ӿ�����Ʈ ���� 
        foreach(GameObject item in _pools[index]) 
        {
            // ���� �߰� �ߴ�, select ������ �Ҵ�
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // ��ã�����, ���Ӱ� �����ؼ� �Ҵ����ش� 
        if(!select) 
        {
            select = Instantiate(_prefabs[index], transform);       // Ǯ �Ŵ��� �ȿ� �ֱ� ���ؼ� transform ���
            _pools[index].Add(select);
        }
        return select;
    }


}
