using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDragServiceReference : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField] private int targetId;
    private ObjectDragServiceOutside dragService;
    
    private void Awake()
    {
        var services = FindObjectsOfType<ObjectDragServiceOutside>();

        foreach (var target in services)
        {
            if (target.ID != targetId) 
                continue;
            
            dragService = target;
            return;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragService.OnDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragService.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragService.OnEndDrag(eventData);
    }
}
