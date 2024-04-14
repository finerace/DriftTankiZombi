using System;
using UnityEngine;

public class PlayerTankObserveReference : NumObserveReference, IObserveNum
{
    private PlayerTank playerTank;

    private void OnEnable()
    {
        playerTank = FindObjectOfType<PlayerTank>();
        
        if(playerTank == null)
            return;
        
        playerTank.OnBarParamChange += () => {OnBarParamChange?.Invoke();};
    }

    public float GetNum(int id)
    {
        return playerTank.GetNum(id);
    }

    public (float min, float max) GetBarParam(int id)
    {
        return playerTank.GetBarParam(id);
    }
    
    public void SetMobileMovementAxis(string axis,float value)
    {
        playerTank.SetMobileMovementAxis(axis,value);
    }
    
    public void SetMobileTankHeadManageData(Vector2 lookDirection, float joystickMagnitude)
    {
        playerTank.SetMobileTankHeadManageData(lookDirection, joystickMagnitude);
    }

    public event Action OnBarParamChange;
    public event Action<int,int> OnObserveNumChange;
}
