using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform _rect;
    Item[] _items;

    private void Awake()
    {
        _rect= GetComponent<RectTransform>();
        _items= GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        _rect.localScale = Vector3.one;
        GameManager._instance.Stop();
        AudioManager._instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager._instance.EffectBgm(true);

    }
    public void Hide()
    {
        _rect.localScale = Vector3.zero;
        GameManager._instance.Resume();
        AudioManager._instance.PlaySfx(AudioManager.Sfx.Selet);
        AudioManager._instance.EffectBgm(false);
    }

    public void Select(int index)
    {
        _items[index].OnClick();
    }

    public void Next()
    {
        // 모든 아이템 비활성화
        foreach(Item item in _items)
        {
            item.gameObject.SetActive(false);
        }
        // 그 중에서 랜덤 3개 아이템 활성화
        int[] ran = new int[3];
        while(true)
        {
            ran[0] = Random.Range(0, _items.Length);
            ran[1] = Random.Range(0, _items.Length);
            ran[2] = Random.Range(0, _items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
            {
                // 중복이 없는 경우 나가기 
                break;
            }
        }

        for(int index = 0; index < ran.Length; index++)
        {
            Item ranItem = _items[ran[index]];

            // 만렙 아이템의 경우 소비 아이템으로 대체 
            if(ranItem._level == ranItem._data._damages.Length) 
            {
                // 소비 아이템이 두개나 세개인 경우 Random.Range(. .) 해주는걸로
                _items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
            
        }
        
    }
}
