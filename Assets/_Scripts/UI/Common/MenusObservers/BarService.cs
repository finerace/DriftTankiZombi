using System;
using UnityEngine;
using UnityEngine.UI;

public class BarService : MonoBehaviour
{
    [SerializeField] private Image bar;

    [Space] 
    
    [SerializeField] private NumObserveReference IbarNumReference;
    [SerializeField] private int numId;

    private IObserveNum burNum;
    private float min;
    private float max;

    [SerializeField] private float speed;
    [SerializeField] private bool inverted;
    
    private void Start()
    {
        burNum = IbarNumReference.GetComponent<IObserveNum>();
        
        UpdateBarParam();
        BarInstantMove();
        burNum.OnBarParamChange += UpdateBarParam;
    }
    
    private void UpdateBarParam()
    {
        if(burNum == null)
            return;
        
        var barParam = burNum.GetBarParam(numId);

        min = barParam.min;
        max = barParam.max;
    }
    
    private void Update()
    {
        BarMove();
        void BarMove()
        {
            var num = burNum.GetNum(numId);
            var trueMax = 0f;

            if (!inverted)
            {
                num -= min;
                trueMax = max - min;
            }
            else
            {
                num -= max;
                trueMax = min - max;
            }
            
            var targetFillAmount = num / trueMax;
            var timeStep = Time.deltaTime * speed;

            bar.fillAmount = Mathf.Lerp(bar.fillAmount, targetFillAmount, timeStep);
        }
    }

    private void OnEnable()
    {
        if(burNum == null)
            return;
        
        UpdateBarParam();
        BarInstantMove();
    }
    
    private void BarInstantMove()
    {
        var num = burNum.GetNum(numId);
        num -= min;

        var trueMax = max - min;

        var targetFillAmount = num / trueMax;
        bar.fillAmount = targetFillAmount;
    }
    
}

interface IObserveNum
{
    float GetNum(int id);
    (float min, float max) GetBarParam(int id);

    event Action OnBarParamChange;
    event Action<int, int> OnObserveNumChange;
}

public abstract class NumObserveReference : MonoBehaviour
{
    
}
