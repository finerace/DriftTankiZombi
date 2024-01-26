using System;
using UnityEngine;

public class LevelsLoadService : MonoBehaviour
{
    [SerializeField] private CommonGameStates commonGameStates;
    [SerializeField] private Transform levelSpawnPoint;
    [SerializeField] private Transform playerTankT;
    
    [Space]
    
    [SerializeField] private LevelData currentLevelData;
    private GameObject currentLevel;

    public void StartLevel(LevelData levelData)
    {
        if(currentLevel != null)
            Destroy(currentLevel);

        currentLevelData = levelData;
        currentLevel = Instantiate(currentLevelData.Prefab, levelSpawnPoint.position,Quaternion.identity);

        playerTankT.position = levelSpawnPoint.position;
        playerTankT.rotation = levelSpawnPoint.rotation;
        
        commonGameStates.SetLevelStartState(true);
    }
    
    private void StopLevel()
    {
        currentLevelData = null;
        
        if(currentLevel != null)
            Destroy(currentLevel);
        
        commonGameStates.SetLevelStartState(false);
    }

    public void SetNewLevelData(LevelData levelData)
    {
        if (currentLevelData == null)
            currentLevelData = levelData;
        else throw new FieldAccessException("Current level is not stopped! New level data not be approved!");
    }

}
