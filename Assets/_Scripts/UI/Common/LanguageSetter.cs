using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LanguageSetter : MonoBehaviour
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

            dropDown.value = YandexGame.savesData.languageId;
        }
    }

    private void Start()
    {
        if(dropDown != null)
            dropDown.onValueChanged.AddListener(Work);
    }

    private void Work(int id)
    {
        switch (id)
        {
            case -1:
                Work(GetLanguageId(YandexGame.savesData.language));
                break;
                
            default:
                CurrentLanguageData.instance.SetLanguageData(id);
                break;
            
        }

        int GetLanguageId(string textId)
        {
            return textId switch
            {
                "en" => 0,
                "ru" => 1,
                "ar" => 2,
                "ca" => 3,
                "es" => 4,
                "hi" => 5,
                "it" => 6,
                "ko" => 7,
                "ro" => 8,
                "th" => 9,
                "uz" => 10,
                "az" => 11,
                "cs" => 12,
                "fa" => 13,
                "hu" => 14,
                "ja" => 15,
                "nl" => 16,
                "tk" => 17,
                "vi" => 18,
                "be" => 19,
                "de" => 20,
                "fr" => 21,
                "hy" => 22,
                "ka" => 23,
                "pl" => 24,
                "sk" => 25,
                "tr" => 26,
                "zh" => 27,
                "bg" => 28,
                "he" => 29,
                "id" => 30,
                "pt" => 31,
                "sr" => 32,
                "uk" => 33,
                _ => 0
            };
        }
        
    }

}
