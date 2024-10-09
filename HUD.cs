using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    // 열거형 enum으로 선언 
    public enum InfoType {  Exp, Level, Kill, Time, Health}
    public InfoType _type;

    Text _myText;
    Slider _mySlider;

    void Awake()
    {
        _myText= GetComponent<Text>();
        _mySlider= GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch(_type)
        {
            case InfoType.Exp:
                // 현재 경험치와 맥스 경험치를 나눠주면 값이 나옴
                float curExp = GameManager._instance._exp;
                float maxExp = GameManager._instance._nextExp[Mathf.Min(GameManager._instance._level, GameManager._instance._nextExp.Length - 1)];
                _mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                //GameManager._instance._level.ToString(); 앞에 Lv.은 따로 ui로 두고 이걸 추가해서 하는거나 아래로 하는거나 같을 거 같음.
                _myText.text = string.Format("Lv.{0:F0}", GameManager._instance._level); 
                break;
            case InfoType.Kill:
                _myText.text = string.Format("KILL.{0:F0}", GameManager._instance._kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager._instance._maxGameT - GameManager._instance._gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);    // 나머지값이 즉 초이기 때문에 
                _myText.text = string.Format("{0:D2} : {1:D2}", min, sec);  // 자릿수를 위해서 D2
                break;
            case InfoType.Health:
                float curHealth = GameManager._instance._health;
                float maxHealth = GameManager._instance._maxHealth;
                _mySlider.value = curHealth / maxHealth;
                break;

        }
    }
}
