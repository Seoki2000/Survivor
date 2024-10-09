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
        // ���������� ���ֱ� ���ؼ�
        _rate = rate;
        ApplyGear();
    }

    // ȣ���� ����ϴ� �Լ��� ������� 
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
        // ó���� �� �������ڸ��� ��� ������� ���� ������Ѿ��ؼ� RateUp���ٰ� ������
        // ������� �ø��� ���� �ٵ� ��� ���� �������
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons)
        {
            // ���Ÿ� �ٰŸ��� ���� �ٸ��� ���
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
        // ����� �÷��̾� OnEnable�� �α����� ����ּ� �̵��ӵ��� �߰��� ����
        float speed = 3 * Character.Speed;
        GameManager._instance._player._speed = speed + speed * _rate;   
    }
}
