using System;

public class PlayerTankObserveReference : NumObserveReference, IObserveNum
{
    private PlayerTank playerTank;

    private void Awake()
    {
        playerTank = FindObjectOfType<PlayerTank>();

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

    public event Action OnBarParamChange;
    public event Action<int,int> OnObserveNumChange;
}
