using TMPro;
using UnityEngine;

public class EarnedMoneyObserver : MonoBehaviour
{
    private LevelScoreCounter levelScoreCounter;
    
    [SerializeField] private TMP_Text earnedMoney;
    [SerializeField] private TMP_Text earnedDonateMoney;

    private void OnEnable()
    {
        if(levelScoreCounter == null)
            levelScoreCounter = LevelScoreCounter.instance;
        
        if(earnedMoney != null)
            earnedMoney.text = levelScoreCounter.EarnedMoney.ToShortenInt();
        
        if(earnedDonateMoney != null)
            earnedDonateMoney.text = levelScoreCounter.EarnedDonateMoney.ToShortenInt();
    }
}
