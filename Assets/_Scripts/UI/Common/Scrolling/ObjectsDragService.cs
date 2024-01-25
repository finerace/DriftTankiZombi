using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ObjectsDragService : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField] private Transform parentPoint;
    [SerializeField] private Transform[] objects;
    [SerializeField] private Transform startPoint;

    [SerializeField] private Vector2 objectsDistance;
    [SerializeField] private float scrollSpeed;
    private bool isBeginDrag;
    private int targetObjectNum;
    
    private void Awake()
    {
        SortObjects();
        void SortObjects()
        {
            objects[0].position = parentPoint.position;
            parentPoint.position = startPoint.position;
            
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].localPosition = (Vector3)objectsDistance * i;
            }
        }
    }

    private void Update()
    {
        if(!isBeginDrag)
            MoveParentPoint();

        void MoveParentPoint()
        {
            var timeStep = Time.deltaTime * scrollSpeed;
            var targetPos = startPoint.localPosition + -(Vector3)objectsDistance * targetObjectNum;
            
            parentPoint.localPosition = 
                Vector3.Lerp(parentPoint.localPosition,targetPos, timeStep);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var trueDelta = Vector2.Dot(objectsDistance.normalized, eventData.delta.normalized) * objectsDistance.normalized *
                        eventData.delta.magnitude;

        parentPoint.localPosition += (Vector3)trueDelta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isBeginDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        targetObjectNum = FindNearestObject();
        int FindNearestObject()
        {
            if (Vector2.Dot((startPoint.position - parentPoint.localPosition).normalized, objectsDistance.normalized) < 0)
                return 0;
            
            var unRoundNearestNum = ((Vector2)parentPoint.localPosition).magnitude / objectsDistance.magnitude;
            var nearestNum = Mathf.RoundToInt(unRoundNearestNum);

            if (nearestNum > objects.Length-1)
                return objects.Length-1;
            
            return nearestNum;
        }
        
        isBeginDrag = false;
    }
}
