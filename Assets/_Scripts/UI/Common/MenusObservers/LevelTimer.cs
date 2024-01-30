using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    private LevelScoreCounter levelScoreCounter;
    [SerializeField] private TMP_Text timerLabel;
    
    private void Start()
    {
        levelScoreCounter = LevelScoreCounter.instance;
    }

    private void Update()
    {
        TimerWork();
        void TimerWork()
        {
            timerLabel.text = levelScoreCounter.LevelCompleteTime.ConvertSecondsToTimer();
        }
    }
}
