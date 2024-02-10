using System;

public class PlayerMoneyXpServiceReference : NumObserveReference, IObserveNum
{
    private PlayerMoneyXpService playerMoneyXp;

    private void Start()
    {
        playerMoneyXp = PlayerMoneyXpService.instance;

        playerMoneyXp.OnBarParamChange += () => {OnBarParamChange?.Invoke();};
        playerMoneyXp.OnObserveNumChange += (int id, int differences) => {OnObserveNumChange?.Invoke(id,differences);};
    }

    public float GetNum(int id)
    {
        return playerMoneyXp.GetNum(id);
    }

    public (float min, float max) GetBarParam(int id)
    {
        return playerMoneyXp.GetBarParam(id);
    }

    public event Action OnBarParamChange;
    public event Action<int, int> OnObserveNumChange;
}
