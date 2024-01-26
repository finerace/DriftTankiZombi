using System;
using UnityEngine;

public class CommonGameStates : MonoBehaviour
{

    [SerializeField] private bool isLevelStarted;
    public event Action<bool> levelStartChangeState;
    
    private LevelData currentLevelData;
    public LevelData CurrentLevelData => currentLevelData;
    
    public void SetLevelStartState(bool state)
    {
        isLevelStarted = state;
        levelStartChangeState?.Invoke(isLevelStarted);
    }

}
