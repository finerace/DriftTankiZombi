using System;
using UnityEngine;

public class DeltaTransformer : MonoBehaviour
{

    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform child;

    [SerializeField] private Vector2 multipliers = Vector2.one;
    
    private void OnEnable()
    {
        var childSizeDelta = child.sizeDelta;
        var sizeDelta = parent.sizeDelta;
        
        childSizeDelta.x = multipliers.x / (Screen.width / Screen.height);
        //childSizeDelta.y = sizeDelta.y * multipliers.y;
        
        child.sizeDelta = childSizeDelta;
    }
}
