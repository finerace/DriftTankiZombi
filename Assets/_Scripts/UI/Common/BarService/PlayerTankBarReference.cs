using System;

public class PlayerTankBarReference : BarReference, IBarNum
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
}
