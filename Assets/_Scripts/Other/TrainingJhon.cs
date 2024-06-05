using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TrainingJhon : MonoBehaviour
{

    [SerializeField] private TMP_Text descLabel;
    [SerializeField] private Transform descLabelT;
    private Vector3 defaultLabelTScale;

    [Space] 
    
    [SerializeField] private int targetLvl;
    [SerializeField] private int targetTrainingLvl;
    [SerializeField] private int[] trainingTextIds;

    private int currentStage;

    [Space] 
    
    [SerializeField] private GameObject trainingPanel;

    private int currentLevel;
    
    private void Awake()
    {
        defaultLabelTScale = descLabelT.localScale;

        LevelsLoadPassService.instance.OnLevelLoad += TrainingStart;
        void TrainingStart()
        {
            currentLevel = LevelsLoadPassService.instance.CurrentLevelData.Id;
            
            currentStage = -1;
            
            switch (currentLevel)
            {
                case 0:
                    YG.YandexGame.savesData.trainingStage = -1;
                    break;
            }
            
            if (YG.YandexGame.savesData.trainingStage >= targetTrainingLvl)
                return;
         
            Continue();
        }


        LevelsLoadPassService.instance.StartCoroutine(Disabler());
        IEnumerator Disabler()
        {
            yield return null;
            trainingPanel.SetActive(false);
        }
    }

    public void Continue()
    {
        if (LevelsLoadPassService.instance.CurrentLevelData.Id != targetLvl)
            return;
    
        currentStage++;
        if (currentStage >= trainingTextIds.Length)
        {
            trainingPanel.SetActive(false);
            YG.YandexGame.savesData.trainingStage = currentLevel;
            return;
        }
        
        print("abba");
        trainingPanel.SetActive(true);
        
        descLabel.text = CurrentLanguageData.GetText(trainingTextIds[currentStage]);

        descLabelT.localScale = defaultLabelTScale * 0.1f;
        descLabelT.DOScale(defaultLabelTScale, 0.25f);
    }

}
