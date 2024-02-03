using System;
using UnityEngine;

public class BaseBounceService : MonoBehaviour
{
    [SerializeField] private Transform meshT;
    [SerializeField] private float startCooldown;
    
    [Space] 
    
    [SerializeField] public bool isBounceActive;
    [SerializeField] private float animationSpeed;
    [SerializeField] private float cooldown;
    private float cooldownTimer;
    
    [Space]
    
    [SerializeField] private float hightTargetValueMultiplier;
    private float defaultHight;
    private float currentHightTargetValue;
    
    
    [SerializeField] private float widthTargetValueMultiplier; 
    private float defaultWidth;
    private float currentWidthTargetValue;

    protected void Start()
    {
        SetFields();
        void SetFields()
        {
            if (meshT == null)
                meshT = transform;

            var meshLocalScale = meshT.localScale;
            defaultWidth = meshLocalScale.x;
            defaultHight = meshLocalScale.y;

            currentHightTargetValue = 1;
            currentWidthTargetValue = 1;

            cooldownTimer = cooldown;
        }

        WorkStartCooldown();
        void WorkStartCooldown()
        {
            if (startCooldown > 0)
                cooldownTimer = startCooldown;
        }
    }

    protected void Update()
    {
        AnimationProcess();
        void AnimationProcess()
        {
            var shopItemLocalScale = meshT.localScale;
            var timeStep = Time.unscaledDeltaTime * animationSpeed;
            
            shopItemLocalScale.x = 
                Mathf.Lerp(shopItemLocalScale.x, currentWidthTargetValue * defaultWidth, timeStep);

            shopItemLocalScale.y =
                Mathf.Lerp(shopItemLocalScale.y, currentHightTargetValue * defaultHight, timeStep);

            meshT.localScale = shopItemLocalScale;
            
            currentHightTargetValue = Mathf.Lerp(currentHightTargetValue,1,timeStep);
            currentWidthTargetValue = Mathf.Lerp(currentWidthTargetValue, 1, timeStep);
        }

        if(!isBounceActive)
            return;
            
        TimerWork();
        void TimerWork()
        {
            cooldownTimer -= Time.unscaledDeltaTime;
            
            if(cooldownTimer <= 0)
            {
                Bounce();
                cooldownTimer = cooldown;
            }
        }
    }

    public void Bounce()
    {
        var hightChange = 0.1f * hightTargetValueMultiplier;
        var widthChange = 0.1f * widthTargetValueMultiplier;

        currentHightTargetValue += hightChange;
        currentWidthTargetValue += widthChange;
    }

    public void ManualBounce(float hightBounce,float widthBounce)
    {
        var hightChange = 0.1f * hightBounce;
        var widthChange = 0.1f * widthBounce;

        currentHightTargetValue += hightChange;
        currentWidthTargetValue += widthChange;
    }
    
    // private void ChangeHightWidth(float hightChange, float widthChange)
    // {
    //     hightChange *= hightTargetValueMultiplier;
    //     widthChange *= widthTargetValueMultiplier;
    //
    //     currentHightTargetValue += hightChange;
    //     currentWidthTargetValue += widthChange;
    // }

}
