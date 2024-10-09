using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �޴��� �������� �ؼ� ������Ʈ�� ���� �����̴�.
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{

    public enum ItemType { Melee, Range, Glove, Shoe, Heal } // ���� ���Ÿ� �尩 �Ź� ����

    [Header("Main Info")]
    public ItemType _itemType;
    public int _itemId;
    public string _itemName;
    [TextArea]
    public string _itemDesc;
    public Sprite _itemIcon;

    [Header("Level Info")]
    public float _baseDamage;
    public int _baseCount;
    public float[] _damages;
    public int[] _count;

    [Header("Weapon Info")]
    public GameObject _projectile;  // ����ü �������� ���ؼ�
    public Sprite _hand;
}
