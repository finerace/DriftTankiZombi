using System;
using UnityEngine;

public class LevelCompleteZone : MonoBehaviour
{

    private LevelsLoadPassService levelsLoadPassService;

    private void Start()
    {
        levelsLoadPassService = FindObjectOfType<LevelsLoadPassService>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3 && other.isTrigger == false)
            levelsLoadPassService.CompleteLevel();
    }
}
