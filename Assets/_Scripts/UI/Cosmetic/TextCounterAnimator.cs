using System;
using TMPro;
using UnityEngine;

public class TextCounterAnimator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool useShortNum = false;
    [SerializeField] private AnimationType animationType;
    [SerializeField] private bool currentNumNotIgnored;
    
    private TMP_Text target;
    private float min;
    private float max;
    
    private bool isWork;
    private float currentNum;
    
    private void Update()
    {
        if(isWork)
            AnimationWork();
        
        void AnimationWork()
        {
            if (animationType == AnimationType.integer)
            {
                currentNum = 
                    Mathf.MoveTowards(currentNum, max, speed * Time.unscaledDeltaTime);

                target.text = !useShortNum ? Mathf.RoundToInt(currentNum).ToString() : Mathf.RoundToInt(currentNum).ToShortenInt();
            }
            else
            {
                currentNum = Mathf.MoveTowards(currentNum, max, speed);
                target.text = $"x{currentNum.ConvertToString()}";
            }

            if (currentNum >= max - 0.01f && currentNum <= max + 0.01f)
                isWork = false;
        }
    }

    public void PlayAnimation(TMP_Text newTarget, float min, float max, float animationTime, AnimationType animationType)
    {
        target = newTarget;

        this.min = min;
        this.max = max;
        
        if (animationTime <= 0)
        {
            if (animationType == AnimationType.integer)
            {
                target.text = !useShortNum ? ((int)max).ToString() : ((int)max).ToShortenInt();
            }
            else
                target.text = $"x{min.ConvertToString()}";

            isWork = false;
            return;
        }
        
        if(!currentNumNotIgnored)
            currentNum = min;
        
        speed = (max - currentNum) / animationTime;
        speed = Mathf.Abs(speed);
        
        if (animationType == AnimationType.floating)
            speed /= 10;

        this.animationType = animationType;
        
        StartAnimation();
    }

    public void StopAnimate()
    {
        isWork = false;
    }

    public void SetCurrentNum(float newNum)
    {
        currentNum = newNum;
    }
    
    private void StartAnimation()
    {
        if (animationType == AnimationType.integer)
        {
            target.text = !useShortNum ? ((int)min).ToString() : ((int)min).ToShortenInt();
        }
        else
            target.text = $"x{min.ConvertToString()}";
        
        isWork = true;
    }
    
}

public enum AnimationType
{
    integer,
    floating
}
