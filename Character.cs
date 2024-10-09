using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        get { return GameManager._instance._playerId == 0 ? 1.1f : 1f; }
    }

    public static float WeaponSpeed
    {
        get { return GameManager._instance._playerId == 1 ? 1.1f : 1f; }
    }

    // 이런식으로 추가해서 다른 캐릭터들도 추가 후, 플레이어에 OnEnable 및 Gear에서 추가해준다.
    public static float WeaponRate
    {
        get { return GameManager._instance._playerId == 1 ? 0.95f : 1f; }
    }

    /*public static float Damage
    {
        get { return GameManager._instance._playerId == 2 ? 1.2f : 1f; }
    }

    public static float Count
    {
        get { return GameManager._instance._playerId == 3 ? 1.2f : 1f; }
    }*/
}
