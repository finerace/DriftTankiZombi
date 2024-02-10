using System;
using TMPro;
using UnityEngine;

public class TextCounterAnimator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool useShortNum = false;
    [SerializeField] private AnimationType animationType;

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
                    Mathf.RoundToInt(Mathf.MoveTowards(currentNum, max, speed * Time.unscaledDeltaTime));

                target.text = !useShortNum ? ((int)currentNum).ToString() : ((int)currentNum).ToShortenInt();
            }
            else
            {
                currentNum = Mathf.MoveTowards(currentNum, max, speed);
                target.text = $"x{currentNum.ConvertToString()}";
            }
            
            if (currentNum >= max)
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
                target.text = !useShortNum ? ((int)min).ToString() : ((int)min).ToShortenInt();
            }
            else
                target.text = $"x{min.ConvertToString()}";
            
            return;
        }
        
        speed = (max - min) / animationTime;
        if (animationType == AnimationType.floating)
            speed /= 10;

        this.animationType = animationType;
        
        StartAnimation();
    }

    private void StartAnimation()
    {
        if (animationType == AnimationType.integer)
        {
            target.text = !useShortNum ? ((int)min).ToString() : ((int)min).ToShortenInt();
        }
        else
            target.text = $"x{min.ConvertToString()}";

        currentNum = min;
        isWork = true;
    }
    
}

public enum AnimationType
{
    integer,
    floating
}
