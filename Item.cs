using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // 아이템 관리에 필요한 변수 선언 
    public ItemData _data;
    public int _level;
    public Weapon _weapon;
    public Gear _gear;

    // 결국 ui에 보여줘야하기 떄문에 이미지 한개 텍스트 한개 가져올거임
    Image _icon;
    Text _textLevel;
    Text _textName;
    Text _textDesc;

    private void Awake()
    {
        _icon = GetComponentsInChildren<Image>()[1];
        _icon.sprite = _data._itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        _textLevel = texts[0];
        _textName = texts[1];
        _textDesc = texts[2];
        _textName.text = _data._itemName;
    }

    void OnEnable()
    {
        _textLevel.text = "Lv." + (_level + 1);

        switch (_data._itemType)
        {
            // 아이템 종류에 따라서 설명이 두줄이 있는 경우 한줄인 경우 따로 해주기 위해서
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                _textDesc.text = string.Format(_data._itemDesc, _data._damages[_level] * 100, _data._count[_level]);
                // 데미지 상승률을 보여주기 위해서 * 100을 했다.
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                _textDesc.text = string.Format(_data._itemDesc, _data._damages[_level] * 100);      // 여기에 100을 안해줘서 0.1퍼 이런식으로 나왔음.
                break;
            default:
                _textDesc.text = string.Format(_data._itemDesc);
                break;
        }

        
    }


    public void OnClick()
    {
        switch (_data._itemType)
        {
            // 처음 누를 때 무기를 만들면 된다.
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if(_level == 0 )
                {
                    GameObject newWeapon = new GameObject();
                    _weapon = newWeapon.AddComponent<Weapon>();
                    _weapon.Init(_data);
                }
                else
                {
                    float nextDamage = _data._baseDamage;
                    int nextCount = 0;
                    // 데미지 같은 경우는 기존에 더한다. 
                    nextDamage += _data._baseDamage * _data._damages[_level];   // 백분율로 해둬서 곱해준것이다.
                    // 카운트는 데이터에 그대로 더한다.
                    nextCount += _data._count[_level];

                    _weapon.LevelUp(nextDamage, nextCount);
                }
                _level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (_level == 0)
                {
                    // 레벨 0 때는 똑같이 만들어서 붙여준다.
                    GameObject newGear = new GameObject();
                    _gear = newGear.AddComponent<Gear>();
                    _gear.Init(_data);
                }
                else
                {
                    float nextRate = _data._damages[_level];
                    _gear.LevelUp(nextRate);
                }
                _level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager._instance._health = GameManager._instance._maxHealth;
                break;

        }

        // 이렇게 할 경우 힐템은 length가 0 이여서 임시로 100까지 하거나 레벨을 늘려주거나 조건을 추가해야 할 것 같다
        if (_level == _data._damages.Length)     
        {
            // 버튼 조정
            GetComponent<Button>().interactable= false;
        }

    }
}
