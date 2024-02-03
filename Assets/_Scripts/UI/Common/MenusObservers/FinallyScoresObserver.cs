using System.Collections;
using TMPro;
using UnityEngine;

public class FinallyScoresObserver : MonoBehaviour
{
    private LevelScoreCounter scoreCounter;

    [SerializeField] private float oneActAnimationTime;
    [SerializeField] private TextCounterAnimator textCounterAnimator;
    
    [Space]
    
    [SerializeField] private TMP_Text enemyDamageScore;
    [SerializeField] private TMP_Text environmentDamageScore;
    [SerializeField] private TMP_Text levelCompleteScore;
    
    [SerializeField] private TMP_Text driftMultiplier;
    
    [SerializeField] private TMP_Text completedTime;
    [SerializeField] private TMP_Text completedTimeMultiplier;

    [SerializeField] private TMP_Text finallyCalculates;
    [SerializeField] private TMP_Text finallyScore;

    private Coroutine workCoroutine;
    
    private void Start()
    {
        scoreCounter = LevelScoreCounter.instance;
    }

    private IEnumerator ScoresAnimation()
    {
        var waiter = new WaitForSecondsRealtime(oneActAnimationTime + 0.05f);
        var miniWater = new WaitForSecondsRealtime(oneActAnimationTime /2);
        
        yield return waiter;
        
        textCounterAnimator.PlayAnimation
            (enemyDamageScore,0,scoreCounter.KilledEnemiesScore,oneActAnimationTime,AnimationType.integer);

        yield return waiter;
        yield return miniWater;
        
        textCounterAnimator.PlayAnimation
            (environmentDamageScore,
                0,scoreCounter.EnvironmentDestructionScore,oneActAnimationTime,AnimationType.integer);

        yield return waiter;
        yield return miniWater;

        textCounterAnimator.PlayAnimation
        (levelCompleteScore,
            0, scoreCounter.GetLevelCompleteScore(), oneActAnimationTime, AnimationType.integer);
        
        yield return waiter;
        yield return miniWater;

        textCounterAnimator.PlayAnimation
            (driftMultiplier,
                1f,scoreCounter.TankDriftScoreMultiplier,oneActAnimationTime,AnimationType.floating);

        yield return waiter;
        yield return miniWater;

        completedTime.text = scoreCounter.LevelCompleteTime.ConvertSecondsToTimer();
        
        textCounterAnimator.PlayAnimation
            (completedTimeMultiplier,1f,
                scoreCounter.GetTimeScoreMultiplier(),oneActAnimationTime,AnimationType.floating);

        yield return waiter;
        yield return miniWater;

        finallyCalculates.text =
            $"{scoreCounter.KilledEnemiesScore + scoreCounter.EnvironmentDestructionScore + scoreCounter.GetLevelCompleteScore()} * " +
            $"x{(scoreCounter.TankDriftScoreMultiplier + scoreCounter.GetTimeScoreMultiplier() - 1f).ConvertToString()} =";
        
        yield return miniWater;

        textCounterAnimator.PlayAnimation
            (finallyScore,0,scoreCounter.GetCompletedScore(),oneActAnimationTime,AnimationType.integer);
    }

    public void ResetValues()
    {
        enemyDamageScore.text = "0";
        environmentDamageScore.text = "0";
        levelCompleteScore.text = "0";

        driftMultiplier.text = "x1.00";

        completedTime.text = "00:00:00";
        completedTimeMultiplier.text = "x1.00";

        finallyCalculates.text = "--- * --- =";
        finallyScore.text = "0";
        
        if(workCoroutine != null)
            StopCoroutine(workCoroutine);
        
    }

    private void StartAnimation()
    {
        workCoroutine = StartCoroutine(ScoresAnimation());
    }

    private void OnEnable()
    {
        ResetValues();
        StartAnimation();
    }
}
