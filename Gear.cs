using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType _type;
    public float _rate;

    public void Init(ItemData data)
    {
        // Basic set
        name = "Gear " + data._itemId;
        transform.parent = GameManager._instance._player.transform;
        transform.localScale = Vector3.zero;

        // Property Set
        _type = data._itemType;
        _rate = data._damages[0];
        ApplyGear(); 
    }
    
    public void LevelUp(float rate)
    {
        // 레벨업때도 해주기 위해서
        _rate = rate;
        ApplyGear();
    }

    // 호출을 담당하는 함수를 만들거임 
    void ApplyGear()
    {
        switch (_type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
                case ItemData.ItemType.Shoe:
                SpeedUp(); 
                break;
        }
    }
    void RateUp()
    {
        // 처음에 기어가 생성되자마자 모든 무기들을 전부 적용시켜야해서 RateUp에다가 적어줌
        // 연사력을 올리기 위해 근데 모든 무기 연사력임
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons)
        {
            // 원거리 근거리에 따라서 다르게 계산
            switch(weapon._id)
            {
                case 0:
                    float speed = 150 * Character.WeaponSpeed;

                    weapon._speed = speed + (150 * _rate); 
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon._speed = speed * (1f - _rate); 
                    break;
            }
        }
    }

    void SpeedUp()
    {
        // 여기와 플레이어 OnEnable에 두군데에 적어둬서 이동속도를 추가로 제어
        float speed = 3 * Character.Speed;
        GameManager._instance._player._speed = speed + speed * _rate;   
    }
}
