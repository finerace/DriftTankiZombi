using System;
using TMPro;
using UnityEngine;
using YG;

public class ObserverDefault : MonoBehaviour
{
    [SerializeField] private NumObserveReference target;
    private IObserveNum observeNum;
    
    [SerializeField] private bool onDataLoadInit;
    private bool isInit;
    
    [SerializeField] private TMP_Text label;
    [SerializeField] private TextCounterAnimator textCounterAnimator;
    
    [Space]
    
    [SerializeField] private int observedNumId;
    [SerializeField] private float animationTime;


    private void Start()
    {
        observeNum = target.GetComponent<IObserveNum>();

        observeNum.OnObserveNumChange += (int id, int difference) =>
        {
            if(!gameObject.activeSelf || id != observedNumId || (onDataLoadInit && !isInit))
                return;

            var num = observeNum.GetNum(observedNumId);
            
            if(animationTime > 0)
                textCounterAnimator.PlayAnimation(label,num - difference, num,animationTime,AnimationType.integer);
            else
                textCounterAnimator.PlayAnimation(label,num, num,0,AnimationType.integer);
        };

        if (onDataLoadInit)
        {
            isInit = YandexGame.savesData != null;
            
            if(!isInit)
                YandexGame.GetDataEvent += SetInit;
            else
                SetNum();
        }
        else
            SetNum();

        void SetInit()
        {
            isInit = true;
            SetNum();
        }
    }

    private void OnEnable()
    {
        SetNum();
    }
    
    private void SetNum()
    {
        if(observeNum == null || YandexGame.savesData == null)
            return;
        
        textCounterAnimator.PlayAnimation(label, observeNum.GetNum(observedNumId), observeNum.GetNum(observedNumId),
            0, AnimationType.integer);
    }
}
