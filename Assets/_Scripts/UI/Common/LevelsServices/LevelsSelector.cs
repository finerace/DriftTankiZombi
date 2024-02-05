using System;
using TMPro;
using UnityEngine;

public class LevelsSelector : MonoBehaviour
{
    private LevelsLoadPassService levelsLoadPassService;
    [SerializeField] private ObjectsDragService dragService;
    [SerializeField] private LevelSelectorData[] levelsData;

    [Space] 
    
    [SerializeField] private TMP_Text levelName;

    private void Start()
    {
        levelsLoadPassService = FindObjectOfType<LevelsLoadPassService>();

        UpdateLevelsUIComponents();

        UpdateLevelName();
        dragService.OnTargetObjectNumChange += UpdateLevelName;
        
        void UpdateLevelName()
        {
            levelName.text = CurrentLanguageData.GetText(levelsData[dragService.TargetObjectNum].LevelData.Name);
        }
    }

    public void StartLevel()
    {
        levelsLoadPassService.LoadLevel(levelsData[dragService.TargetObjectNum].LevelData);
    }

    private void OnEnable()
    {
        if(GameDataSaver.instance != null)
            UpdateLevelsUIComponents();
    }

    private void UpdateLevelsUIComponents()
    {
        foreach (var data in levelsData)
        {
            var levelData = data.LevelData;
            var gameData = GameDataSaver.instance;
            var levelHighScore = gameData.GetLevelHighScore(levelData.Id);

            if(gameData.IsLevelCompleted(data.LevelData.Id))
                StarsUpdate();
            void StarsUpdate()
            {
                if(levelHighScore > levelData.OneStarScore)
                    data.Star1.gameObject.SetActive(true);
                    
                if(levelHighScore > levelData.TwoStarScore)
                    data.Star2.gameObject.SetActive(true);

                if(levelHighScore > levelData.ThreeStarScore)
                    data.Star3.gameObject.SetActive(true);
            }

            UpdateHighScoreLabel();
            void UpdateHighScoreLabel()
            {
                data.LevelHighScore.text = levelHighScore.ToString();
            }
        }
    }
}
