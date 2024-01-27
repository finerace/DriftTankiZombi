using UnityEngine;

public class LevelsLoadServiceReference : MonoBehaviour
{

    private LevelsLoadService levelsLoadService;

    private void Awake()
    {
        levelsLoadService = FindObjectOfType<LevelsLoadService>();
    }

    public void StopLevel()
    {
        levelsLoadService.StopLevel();
    }
    
}
