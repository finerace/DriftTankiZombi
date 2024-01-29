using System;
using TMPro;
using UnityEngine;

public class LevelsSelector : MonoBehaviour
{
    private LevelsLoadPassService levelsLoadPassService;
    [SerializeField] private ObjectsDragService dragService;
    [SerializeField] private LevelData[] levels;

    [Space] 
    
    [SerializeField] private TMP_Text levelName;

    private void Awake()
    {
        levelsLoadPassService = FindObjectOfType<LevelsLoadPassService>();

        UpdateLevelName();
        dragService.OnTargetObjectNumChange += UpdateLevelName;
        
        void UpdateLevelName()
        {
            levelName.text = CurrentLanguageData.GetText(levels[dragService.TargetObjectNum].Name);
        }
    }

    public void StartLevel()
    {
        levelsLoadPassService.LoadLevel(levels[dragService.TargetObjectNum]);
    }
}
