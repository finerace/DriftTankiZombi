using System;
using UnityEngine;

public class CommonGameStates : MonoBehaviour
{

    [SerializeField] private bool isLevelStarted;
    public event Action<bool> levelStartChangeState;

    public void SetLevelStartState(bool state)
    {
        isLevelStarted = state;
        levelStartChangeState?.Invoke(isLevelStarted);
    }

}
