using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // ������ ������ �ʿ��� ���� ���� 
    public ItemData _data;
    public int _level;
    public Weapon _weapon;
    public Gear _gear;

    // �ᱹ ui�� ��������ϱ� ������ �̹��� �Ѱ� �ؽ�Ʈ �Ѱ� �����ð���
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
            // ������ ������ ���� ������ ������ �ִ� ��� ������ ��� ���� ���ֱ� ���ؼ�
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                _textDesc.text = string.Format(_data._itemDesc, _data._damages[_level] * 100, _data._count[_level]);
                // ������ ��·��� �����ֱ� ���ؼ� * 100�� �ߴ�.
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                _textDesc.text = string.Format(_data._itemDesc, _data._damages[_level] * 100);      // ���⿡ 100�� �����༭ 0.1�� �̷������� ������.
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
            // ó�� ���� �� ���⸦ ����� �ȴ�.
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
                    // ������ ���� ���� ������ ���Ѵ�. 
                    nextDamage += _data._baseDamage * _data._damages[_level];   // ������� �صּ� �����ذ��̴�.
                    // ī��Ʈ�� �����Ϳ� �״�� ���Ѵ�.
                    nextCount += _data._count[_level];

                    _weapon.LevelUp(nextDamage, nextCount);
                }
                _level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (_level == 0)
                {
                    // ���� 0 ���� �Ȱ��� ���� �ٿ��ش�.
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

        // �̷��� �� ��� ������ length�� 0 �̿��� �ӽ÷� 100���� �ϰų� ������ �÷��ְų� ������ �߰��ؾ� �� �� ����
        if (_level == _data._damages.Length)     
        {
            // ��ư ����
            GetComponent<Button>().interactable= false;
        }

    }
}
