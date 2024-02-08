using UnityEngine;
using YG;

public class GameDataSaver : MonoBehaviour
{

    public static GameDataSaver instance;
    private SavesYG savesYg;

    private void Awake()
    {
        instance = this;
        
        YandexGame.GetDataEvent += InitSaveData;
        void InitSaveData()
        {
            savesYg = YandexGame.savesData;
        }
    }

    private void Start()
    {
        YandexGame.LoadProgress();
    }

    public void SetNewLeveHighScore(int levelId, int newHighScore)
    {
        savesYg.levelsData[levelId].levelHighScore = newHighScore;

        Save();
    }

    public int GetLevelHighScore(int levelId)
    {
        return savesYg.levelsData[levelId].levelHighScore;
    }

    public void SetLevelLockState(int levelId, bool state)
    {
        if (savesYg.levelsData[levelId].isLevelUnlocked == state) 
            return;
        
        savesYg.levelsData[levelId].isLevelUnlocked = state;

        Save();
    }

    public bool GetLockState(int levelId)
    {
        return instance.savesYg.levelsData[levelId].isLevelUnlocked;
    }

    public void SetLevelCompletedState(int levelId,bool state)
    {
        if (savesYg.levelsData[levelId].isLevelCompleted == state) 
            return;
        
        savesYg.levelsData[levelId].isLevelCompleted = state;
        
        Save();
    }

    public bool IsLevelCompleted(int levelId)
    {
        return instance.savesYg.levelsData[levelId].isLevelCompleted;
    }

    private void Save()
    {
        YandexGame.SaveProgress();
    }
    
}
