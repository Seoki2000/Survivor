using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Hands : MonoBehaviour
{
    public bool _isLeft;
    public SpriteRenderer _spriter;

    SpriteRenderer _playerSpr;

    Vector3 _rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 _rightPosReverse = new Vector3(-0.35f, -0.15f, 0);
    Quaternion _leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion _leftRotReverser = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        _playerSpr = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        bool isReverse = _playerSpr.flipX;

        if(_isLeft) // 근접무기
        {
            transform.localRotation = isReverse ? _leftRotReverser : _leftRot;
            _spriter.flipY = isReverse;
            _spriter.sortingOrder = isReverse ? 9 : 11;
        }
        else
        {
            // 원거리 무기
            transform.localPosition = isReverse ? _rightPosReverse : _rightPos;
            _spriter.flipX = isReverse;
            _spriter.sortingOrder = isReverse ? 11 : 9;
        }
    }
}
