using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 메뉴를 바탕으로 해서 오브젝트를 만들 예정이다.
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{

    public enum ItemType { Melee, Range, Glove, Shoe, Heal } // 근접 원거리 장갑 신발 힐템

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
    public GameObject _projectile;  // 투사체 프리팹을 위해서
    public Sprite _hand;
}
