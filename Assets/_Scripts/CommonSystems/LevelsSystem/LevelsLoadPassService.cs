using System;
using UnityEngine;

public class LevelsLoadPassService : MonoBehaviour
{
    [SerializeField] private CommonGameStates commonGameStates;
    [SerializeField] private Transform levelSpawnPoint;
    [SerializeField] private Transform playerTankT;
    [SerializeField] private GameObject virtualCamera;
    
    [Space] 
    
    [SerializeField] private MenuSystem mainMenu;
    [SerializeField] private MenuSystem gameMenu;
    
    [Space]
    
    [SerializeField] private LevelData currentLevelData;
    private GameObject currentLevel;

    public void LoadLevel(LevelData levelData)
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
        if (currentLevelData == null)
            throw new Exception("Currently level is null!");
        
        gameMenu.OpenLocalMenu("StopLevel");
    }

    public void RestartLevel()
    {
        if (currentLevelData == null)
            throw new Exception("Currently level is null!");
        
        var savedLevelData = currentLevelData;
        
        UnloadLevel();
        LoadLevel(savedLevelData);
    }
    
    public void UnloadLevel()
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

    public void CompleteLevel()
    {
        if (currentLevelData == null)
            throw new Exception("Currently level is null!");
        
        gameMenu.OpenLocalMenu("LevelComplete");
    }
    
    private void SetMainMenuActive(bool state)
    {
        mainMenu.gameObject.SetActive(state);
        if(state)
            mainMenu.OpenStartMenu();
        
        gameMenu.gameObject.SetActive(!state);
        if(!state)
            gameMenu.OpenStartMenu();
    }
    
}
