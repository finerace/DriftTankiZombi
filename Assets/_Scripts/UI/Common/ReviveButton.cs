using System;
using UnityEngine;

public class ReviveButton : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private void OnEnable()
    {
        var scores = LevelScoreCounter.instance;
        
        target.SetActive(scores.reviveCount <= LevelScoreCounter.maxReviveCount);
    }
}
