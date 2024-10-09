using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    RectTransform _rect;
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();  
    }

    private void FixedUpdate()
    {
        _rect.position = Camera.main.WorldToScreenPoint(GameManager._instance._player.transform.position);
        
    }
}
