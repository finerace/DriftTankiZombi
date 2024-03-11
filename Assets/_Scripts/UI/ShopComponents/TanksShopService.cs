using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TanksShopService : MonoBehaviour
{
    [SerializeField] private ShopData[] tanksShopDatas;
    [SerializeField] private ObjectsDragService objectsDragService;

    private PlayerMoneyXpService playerMoneyXpService;
    
    [Space] 
    
    [SerializeField] private TMP_Text tankName;

    [SerializeField] private Image charManage;
    [SerializeField] private Image charGun;
    [SerializeField] private Image charEngine;
    [SerializeField] private Image charFuel;
    
    [SerializeField] private TMP_Text engineUpgradesLabel;
    [SerializeField] private TMP_Text gunUpgradesLabel;
    [SerializeField] private TMP_Text fuelUpgradesLabel;

    [SerializeField] private TMP_Text engineUpgradePrice;
    [SerializeField] private TMP_Text gunUpgradePrice;
    [SerializeField] private TMP_Text fuelUpgradePrice;

    private void Start()
    {
        playerMoneyXpService = FindObjectOfType<PlayerMoneyXpService>();

        objectsDragService.OnTargetObjectNumChange += SelectNewTank;
        void SelectNewTank()
        {
            var newTankData = tanksShopDatas[objectsDragService.TargetObjectNum];

            tankName.text = CurrentLanguageData.GetText(newTankData.NameLanguageId);

            charManage.fillAmount = newTankData.ManageChar;
            charGun.fillAmount = newTankData.GunChar;
            charEngine.fillAmount = newTankData.SpeedChar;
            charFuel.fillAmount = newTankData.FuelChar;

            engineUpgradesLabel.text = $"";

        }
        
    }
}
