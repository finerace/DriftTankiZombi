using System;
using System.Collections;
using UnityEngine;

public class LevelsLoadPassService : MonoBehaviour
{
    public static LevelsLoadPassService instance;
    
    [SerializeField] private GlobalGameEvents globalGameEvents;
    [SerializeField] private Transform levelSpawnPoint;
    [SerializeField] private Transform playerTankT;
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] private LevelScoreCounter levelScoreCounter;

    [Space] 
    
    [SerializeField] private MenuSystem mainMenu;
    [SerializeField] private MenuSystem gameMenu;
    
    [Space]
    
    [SerializeField] private LevelData currentLevelData;
    private GameObject currentLevel;
    
    private bool isCurrentLevelComplete = false;
    private Coroutine dieCoroutine;

    private GameDataSaver gameDataSaver;
    
    private void Awake()
    {
        instance = this;
        
        SetPlayerDieAlgorithm();
        void SetPlayerDieAlgorithm()
        {
            playerTankT.gameObject.GetComponent<PlayerTank>().OnDie += () =>
            {
                dieCoroutine = StartCoroutine(OnPlayerDie());
            };

            IEnumerator OnPlayerDie()
            {
                yield return new WaitForSeconds(3);

                gameMenu.isBackActionLock = true;
                StopLevel();
            }
        }
    }

    private void Start()
    {
        gameDataSaver = GameDataSaver.instance;
    }

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

        levelScoreCounter.ResetCounters();
        levelScoreCounter.SetNewCurrentLevelData(currentLevelData);
        
        ChangeMenusActivity(false);
        
        globalGameEvents.SetLevelStartState(true);
    }

    public void StopLevel()
    {
        if (currentLevelData == null)
            throw new Exception("Currently level is null!");
        
        SaveHighScore();
        void SaveHighScore()
        {
            var oldHighScore = gameDataSaver.GetLevelHighScore(currentLevelData.Id);
            var currentScore = levelScoreCounter.GetUncompletedScore(); 
            
            if(currentScore > oldHighScore)
                gameDataSaver.SetNewLeveHighScore(currentLevelData.Id,currentScore);
        }

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

        isCurrentLevelComplete = false;
        playerTankT.gameObject.SetActive(false);
        
        ChangeMenusActivity(true);
        gameMenu.isBackActionLock = false;
        
        virtualCamera.SetActive(false);

        DisableDieCoroutine();
        
        levelScoreCounter.SetNewCurrentLevelData(null);
        
        globalGameEvents.SetLevelStartState(false);
    }
    
    public void CompleteLevel()
    {
        if(isCurrentLevelComplete)
            return;

        gameMenu.isBackActionLock = true;
        isCurrentLevelComplete = true;
        
        SaveResults();
        void SaveResults()
        {
            NewHighScoreWork();
            void NewHighScoreWork()
            {
                var oldHighScore = gameDataSaver.GetLevelHighScore(currentLevelData.Id);
                var currentScore = levelScoreCounter.GetCompletedScore(); 
            
                if(currentScore > oldHighScore)
                    gameDataSaver.SetNewLeveHighScore(currentLevelData.Id,currentScore);
            }

            if (!gameDataSaver.IsLevelCompleted(currentLevelData.Id))
                gameDataSaver.SetLevelCompletedState(currentLevelData.Id,true);
        }
        
        DisableDieCoroutine();
        
        if (currentLevelData == null)
            throw new Exception("Currently level is null!");
        
        gameMenu.OpenLocalMenu("LevelComplete");
    }

    private void DisableDieCoroutine()
    {
        if (dieCoroutine != null)
            StopCoroutine(dieCoroutine);
    }

    private void ChangeMenusActivity(bool isMainMenuActive)
    {
        mainMenu.gameObject.SetActive(isMainMenuActive);
        if(isMainMenuActive)
            mainMenu.OpenStartMenu();
        
        gameMenu.gameObject.SetActive(!isMainMenuActive);
        if(!isMainMenuActive)
            gameMenu.OpenStartMenu();
    }
    
    /*public void SetNewLevelData(LevelData levelData)
    {
        if (currentLevelData == null)
            currentLevelData = levelData;
        else throw new FieldAccessException("Current level is not stopped! New level data not be approved!");
    }*/
}
