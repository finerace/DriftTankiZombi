using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class QualitySetter : MonoBehaviour
{
    private Dropdown dropDown;
    
    private void OnEnable()
    {
        if (YandexGame.SDKEnabled)
            Init();
        else
            YandexGame.GetDataEvent += Init;
        
        void Init()
        {
            if(dropDown == null)
                dropDown = GetComponent<Dropdown>();

            dropDown.value = YandexGame.savesData.qualityId;
        }
    }

    private void Start()
    {
        if(dropDown != null)
            dropDown.onValueChanged.AddListener(Work);
    }

    private void Work(int id)
    {
        QualitySettings.SetQualityLevel(id);
    }

}
