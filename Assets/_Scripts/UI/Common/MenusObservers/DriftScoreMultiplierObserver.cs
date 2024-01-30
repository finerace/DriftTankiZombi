using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DriftScoreMultiplierObserver : MonoBehaviour
{
    private LevelScoreCounter levelScoreCounter;
    [SerializeField] private TMP_Text label;
    [SerializeField] private float upEffectsBarrier;
    private Transform labelT;
    
    [Space]
    
    [SerializeField] private Gradient gradient;
    [SerializeField] private float bounceMaxBounce = 10;
    [SerializeField] private float bounceMaxPower = 20;
    
    private void Start()
    {
        levelScoreCounter = LevelScoreCounter.instance;
        labelT = label.transform;
    }
    
    private void Update()
    {
        var multiplier = levelScoreCounter.TankDriftScoreMultiplier;
        var multiplierRound = (int)multiplier; 
        
        label.text = 
            $"x{(int)multiplier}." +
            $"{((int)((multiplier.ClampToTwoRemainingCharacters() - multiplierRound) * 100)).ConvertNumCharacters()}";
        
        var amount = (multiplier - 1) / (upEffectsBarrier - 1);
        if (amount > 1)
            amount = 1;
        
        ColorUpdate();
        void ColorUpdate()
        {
            label.color = gradient.Evaluate(amount);
        }
        
        BounceUpdate();
        void BounceUpdate()
        {
            var finalZ = 
                Mathf.Cos(levelScoreCounter.LevelCompleteTime * bounceMaxPower) * (bounceMaxBounce * amount);
            
            var labelEuler = labelT.localEulerAngles;
            labelEuler.z = finalZ;
            labelT.localEulerAngles = labelEuler;
        }
    }
}
