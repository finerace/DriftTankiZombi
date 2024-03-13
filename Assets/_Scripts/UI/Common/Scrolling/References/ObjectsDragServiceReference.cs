using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectsDragServiceReference : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField] private int targetId;
    private ObjectDragServiceOutside dragService;
    public event Action OnTargetObjectNumChange;

    public int TargetObjectNum => dragService.TargetObjectNum;

    private void Awake()
    {
        var services = FindObjectsOfType<ObjectDragServiceOutside>();

        foreach (var target in services)
        {
            if (target.ID != targetId) 
                continue;
            
            dragService = target;

            dragService.OnTargetObjectNumChange += () => {OnTargetObjectNumChange?.Invoke();};
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
