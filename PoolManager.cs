using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] _prefabs;

    // 풀 담당을 하는 리스트들 

    List<GameObject>[] _pools;


    private void Awake()
    {
        _pools = new List<GameObject>[_prefabs.Length];
        // 리스트를 순회하면서 초기화 
        for (int index = 0; index < _pools.Length; index++)
        {
            _pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // 선택한 풀의 놀고 있는 게임오브젝트 접근 
        foreach(GameObject item in _pools[index]) 
        {
            // 만약 발견 했다, select 변수에 할당
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 못찾은경우, 새롭게 생성해서 할당해준다 
        if(!select) 
        {
            select = Instantiate(_prefabs[index], transform);       // 풀 매니저 안에 넣기 위해서 transform 사용
            _pools[index].Add(select);
        }
        return select;
    }


}
