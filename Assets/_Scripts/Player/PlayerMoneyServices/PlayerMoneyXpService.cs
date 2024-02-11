using System;
using UnityEngine;
using YG;

public class PlayerMoneyXpService : MonoBehaviour,IObserveNum
{
    public static PlayerMoneyXpService instance;

    public int PlayerMoney
    {
        get => YandexGame.savesData.playerMoney;

        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Value of player coins can not be less than zero!");

            var oldValue = YandexGame.savesData.playerMoney;
            YandexGame.savesData.playerMoney = value;
            
            OnMoneyChange?.Invoke(YandexGame.savesData.playerMoney - oldValue);
        }
    }
    public event Action<int> OnMoneyChange;

    
    public int PlayerDonateMoney
    {
        get => YandexGame.savesData.playerDonateMoney;

        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Value of player coins can not be less than zero!");

            var oldValue = YandexGame.savesData.playerDonateMoney;
            YandexGame.savesData.playerDonateMoney = value;
            
            OnDonateMoneyChange?.Invoke(YandexGame.savesData.playerDonateMoney - oldValue);
        }
    }
    public event Action<int> OnDonateMoneyChange;

    
    public int PlayerXp
    {
        get => YandexGame.savesData.playerXp;

        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Value of player Xp can not be less than zero!");

            var oldValue = YandexGame.savesData.playerXp;
            YandexGame.savesData.playerXp = value;
            
            if (value - oldValue < 0)
                throw new ArgumentOutOfRangeException("Value of player Xp can not be less!");
            
            OnXpChanged?.Invoke(YandexGame.savesData.playerXp - oldValue);
        }
    }
    [SerializeField] private int toNextLevelDefaultXp = 100;
    [SerializeField] private float newLevelCof;
    public event Action<int> OnXpChanged; 

    private void Awake()
    {
        instance = this;

        InitObserveNumChangesEvents();
        void InitObserveNumChangesEvents()
        {
            OnMoneyChange += (int difference) => {OnObserveNumChange?.Invoke(1,difference);};
            OnDonateMoneyChange += (int difference) => {OnObserveNumChange?.Invoke(2,difference);};
            OnXpChanged += (int difference) => {OnObserveNumChange?.Invoke(3,difference);};
        }
    }
    
    public bool TryTakeMoney(int moneyCount)
    {
        if (PlayerMoney < moneyCount) 
            return false;
        
        PlayerMoney -= moneyCount;

        return true;
    }
    
    public bool TryTakeDonateMoney(int moneyCount)
    {
        if (PlayerDonateMoney < moneyCount) 
            return false;
        
        PlayerDonateMoney -= moneyCount;

        return true;
    }

    public void AddXp(int count)
    {
        if (count < 0)
            throw new ArgumentException("Add Xp argument cannot be less than zero!");

        var oldXp = PlayerXp;
        PlayerXp += count;
        
        OnXpChanged?.Invoke(PlayerXp - oldXp);
    }

    private int GetCurrentLevel()
    {
        var currentLevel = 1;
        
        var workXp = PlayerXp;
        
        while (true)
        {
            var toNextLevel = toNextLevelDefaultXp + 
                              Mathf.RoundToInt(toNextLevelDefaultXp * currentLevel * newLevelCof);

            if (workXp >= toNextLevel)
            {
                workXp -= toNextLevel;
                currentLevel++;
            }
            else return currentLevel;
        }
    }

    private float GetToNextLevelAmount()
    {
        var currentLevel = 1;
        
        var workXp = PlayerXp;
        
        while (true)
        {
            var toNextLevel = toNextLevelDefaultXp + 
                              Mathf.RoundToInt(toNextLevelDefaultXp * currentLevel * newLevelCof);

            if (workXp >= toNextLevel)
            {
                workXp -= toNextLevel;
                currentLevel++;
            }
            else return workXp / (float)toNextLevel;
        }
    }

    public float GetNum(int id)
    {
        switch (id)
        {
            case 1:
                return PlayerMoney;
            case 2:
                return PlayerDonateMoney;
            case 3:
                return GetCurrentLevel();
            case 4:
                return GetToNextLevelAmount();
            default:
                throw new ArgumentException("This Num Id is not exist!");
        }
    }

    public (float min, float max) GetBarParam(int id)
    {
        switch (id)
        {
            case 4:
                return (0,1);
            default:
                throw new ArgumentException("This BarParam Id is not exist!");
        }
    }

    public event Action OnBarParamChange;
    public event Action<int,int> OnObserveNumChange;
}
