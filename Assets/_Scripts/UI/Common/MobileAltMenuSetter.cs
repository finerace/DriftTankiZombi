using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class MobileAltMenuSetter : MonoBehaviour
{
    [SerializeField] private GameObject menu1;
    [SerializeField] private GameObject menu2;

    private void OnEnable()
    {
        menu1.SetActive(!YandexGame.savesData.mobileManageAlternativeMenu);
        menu2.SetActive(YandexGame.savesData.mobileManageAlternativeMenu);

    }
}
