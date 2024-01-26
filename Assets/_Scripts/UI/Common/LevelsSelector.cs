using System;
using UnityEngine;

public class LevelsSelector : MonoBehaviour
{
    private LevelsLoadService levelsLoadService;
    [SerializeField] private ObjectsDragService dragService;
    [SerializeField] private LevelData[] levels;
    
    private void Awake()
    {
        levelsLoadService = FindObjectOfType<LevelsLoadService>();
    }

    public void StartLevel()
    {
        levelsLoadService.StartLevel(levels[dragService.TargetObjectNum]);
    }
}
