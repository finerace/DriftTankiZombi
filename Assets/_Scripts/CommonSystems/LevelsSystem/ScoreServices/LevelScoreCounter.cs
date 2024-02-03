using System;
using UnityEngine;

public class LevelScoreCounter : MonoBehaviour
{
    public static LevelScoreCounter instance;
    [SerializeField] private TankEffects playerTankEffects;

    [Space] 
    
    [SerializeField] private int environmentDestructionScore = 0;
    [SerializeField] private int killedEnemiesScore = 0;
    [SerializeField] private float tankDriftScoreMultiplier = 1;

    [Space] 
    
    [SerializeField] private float driftScoreMultiplierPower = 0.025f;
    [SerializeField] private float levelCompleteTime;
    
    private LevelData currentLevelData;

    public float TankDriftScoreMultiplier => tankDriftScoreMultiplier;
    public float LevelCompleteTime => levelCompleteTime;
    public int EnvironmentDestructionScore => environmentDestructionScore;
    public int KilledEnemiesScore => killedEnemiesScore;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        DriftScoreMultiplierWork();
        void DriftScoreMultiplierWork()
        {
            if (playerTankEffects.IsTankDrifting)
            {
                tankDriftScoreMultiplier += Time.deltaTime * driftScoreMultiplierPower;
            }
        }

        if(currentLevelData != null)
            levelCompleteTime += Time.deltaTime;
    }

    public void AddEnvironmentDestructionScore(int score)
    {
        if (score <= 0)
            throw new ArgumentException();

        environmentDestructionScore += score;
    }

    public void AddEnemyKilledDestructionScore(int score)
    {
        if (score <= 0)
            throw new ArgumentException();

        killedEnemiesScore += score;
    }
    
    public int GetCompletedScore()
    {
        var finalScore =
            (environmentDestructionScore + killedEnemiesScore + GetLevelCompleteScore()) * 
            (TankDriftScoreMultiplier + GetTimeScoreMultiplier() - 1f);

        return Mathf.RoundToInt(finalScore);
    }

    public int GetUncompletedScore()
    {
        var finalScore =
            (environmentDestructionScore + killedEnemiesScore) * tankDriftScoreMultiplier;

        return Mathf.RoundToInt(finalScore);
    }
    
    public float GetTimeScoreMultiplier()
    {
        var multiplier = currentLevelData.CompleteTime / levelCompleteTime;

        if (multiplier < 1)
            multiplier = 1;

        return multiplier;
    }

    public int GetLevelCompleteScore()
    {
        return currentLevelData.CompleteScore;
    }
    
    public void ResetCounters()
    {
        environmentDestructionScore = 0;
        killedEnemiesScore = 0;
        tankDriftScoreMultiplier = 1;
        levelCompleteTime = 0;
    }

    public void SetNewCurrentLevelData(LevelData levelData)
    {
        currentLevelData = levelData;
    }

}
