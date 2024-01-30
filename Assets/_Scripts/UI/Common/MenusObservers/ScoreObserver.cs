using TMPro;
using UnityEngine;

public class ScoreObserver : MonoBehaviour
{
    private LevelScoreCounter levelScoreCounter;
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        levelScoreCounter = LevelScoreCounter.instance;
    }
    
    private void Update()
    {
        label.text = levelScoreCounter.GetUncompletedScore().ToString();
    }
}
