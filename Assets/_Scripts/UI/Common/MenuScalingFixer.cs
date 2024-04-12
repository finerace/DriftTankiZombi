using System;
using UnityEngine;

public class MenuScalingFixer : MonoBehaviour
{
    private void Awake()
    {
        var rectTrans = GetComponent<RectTransform>();
        
        rectTrans.sizeDelta = Vector2.zero;

        rectTrans.anchorMin = Vector2.zero;
        rectTrans.anchorMax = Vector2.one;
        rectTrans.pivot = Vector2.one / 2f;
    }
}
