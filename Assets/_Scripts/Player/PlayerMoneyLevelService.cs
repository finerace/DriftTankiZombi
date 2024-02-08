using System;
using UnityEngine;

public class PlayerMoneyLevelService : MonoBehaviour
{
    public static PlayerMoneyLevelService instance;
    
    
    [SerializeField] private int playerMoney;
    public int PlayerMoney
    {
        get => playerMoney;

        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Value of player coins can not be less than zero!");

            var oldValue = playerMoney;
            playerMoney = value;
            
            OnMoneyChange.Invoke(playerMoney - oldValue);
        }
    }
    public event Action<int> OnMoneyChange;

    [Space]
    
    [SerializeField] private int playerDonateMoney;
    public int PlayerDonateMoney
    {
        get => playerDonateMoney;

        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Value of player coins can not be less than zero!");

            var oldValue = playerDonateMoney;
            playerMoney = value;
            
            OnDonateMoneyChange.Invoke(playerDonateMoney - oldValue);
        }
    }
    public event Action<int> OnDonateMoneyChange;

    [Space] 
    
    [SerializeField] private int Xp;
    [SerializeField] private int newLevelCof;
    public event Action<int> onXpChanged; 

    private void Awake()
    {
        instance = this;
    }

    public bool TryTakeMoney(int moneyCount)
    {
        if (playerMoney < moneyCount) 
            return false;
        
        PlayerMoney -= moneyCount;

        return true;
    }
    
    public bool TryTakeDonateMoney(int moneyCount)
    {
        if (playerDonateMoney < moneyCount) 
            return false;
        
        PlayerDonateMoney -= moneyCount;

        return true;
    }

    public void AddXp(int count)
    {
        if (count < 0)
            throw new ArgumentException("Add Xp argument cannot be less than zero!");

        var oldXp = Xp;
        Xp += count;
        
        onXpChanged?.Invoke(Xp - oldXp);
    }

    public int GetCurrentLevel()
    {
        var currentLevel = 1;
        
        var workXp = Xp;
        const int toNextLevelDefaultXp = 100;
        
        while (true)
        {
            var toNextLevel = toNextLevelDefaultXp + (toNextLevelDefaultXp * currentLevel * newLevelCof);

            if (workXp >= toNextLevel)
            {
                workXp -= toNextLevel;
                currentLevel++;
            }
            else return currentLevel;
        }
    }
    
}
