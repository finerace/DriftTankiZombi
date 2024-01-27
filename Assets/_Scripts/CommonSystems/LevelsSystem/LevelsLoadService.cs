using System;
using UnityEngine;

public class LevelsLoadService : MonoBehaviour
{
    [SerializeField] private CommonGameStates commonGameStates;
    [SerializeField] private Transform levelSpawnPoint;
    [SerializeField] private Transform playerTankT;
    [SerializeField] private GameObject virtualCamera;
    
    [Space] 
    
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameMenu;
    
    [Space]
    
    [SerializeField] private LevelData currentLevelData;
    private GameObject currentLevel;

    public void StartLevel(LevelData levelData)
    {
        if(currentLevel != null)
            Destroy(currentLevel);

        currentLevelData = levelData;
        currentLevel = Instantiate(currentLevelData.Prefab, levelSpawnPoint.position,Quaternion.identity);

        SpawnPlayer();
        void SpawnPlayer()
        {
            playerTankT.position = levelSpawnPoint.position;
            playerTankT.rotation = levelSpawnPoint.rotation;

            playerTankT.gameObject.SetActive(true);
        }
        
        virtualCamera.SetActive(true);
        
        SetMainMenuActive(false);
        
        commonGameStates.SetLevelStartState(true);
    }
    
    public void StopLevel()
    {
        currentLevelData = null;
        if(currentLevel != null)
            Destroy(currentLevel);

        playerTankT.gameObject.SetActive(false);
        SetMainMenuActive(true);
        
        virtualCamera.SetActive(false);
        
        commonGameStates.SetLevelStartState(false);
    }
    
    public void SetNewLevelData(LevelData levelData)
    {
        if (currentLevelData == null)
            currentLevelData = levelData;
        else throw new FieldAccessException("Current level is not stopped! New level data not be approved!");
    }

    private void SetMainMenuActive(bool state)
    {
        mainMenu.SetActive(state);
        gameMenu.SetActive(!state);
    }
    
}
