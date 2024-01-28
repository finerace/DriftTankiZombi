using UnityEngine;

public class LevelsLoadPassServiceReference : MonoBehaviour
{

    private LevelsLoadPassService levelsLoadPassService;

    private void Awake()
    {
        levelsLoadPassService = FindObjectOfType<LevelsLoadPassService>();
    }

    public void StopLevel()
    {
        levelsLoadPassService.UnloadLevel();
    }

    public void RestartLevel()
    {
        levelsLoadPassService.RestartLevel();
    }
    
}
