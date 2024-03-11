using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

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
    [SerializeField] private TMP_Text gunsUpgradesLabel;
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
            var currentNum = objectsDragService.TargetObjectNum;
            
            var newTankData = tanksShopDatas[currentNum];
            var newTankSaveData = YandexGame.savesData.tanksData[currentNum];
            
            tankName.text = CurrentLanguageData.GetText(newTankData.NameLanguageId);
            
            SetCharacteristicsFillAmount();
            void SetCharacteristicsFillAmount()
            {
                charManage.fillAmount = newTankData.ManageChar;
                charGun.fillAmount = newTankData.GunChar;
                charEngine.fillAmount = newTankData.SpeedChar;
                charFuel.fillAmount = newTankData.FuelChar;
            }

            SetUpgradesLabel();
            void SetUpgradesLabel()
            {
                engineUpgradesLabel.text = $"{newTankSaveData.engineImprovement}/" +
                                           $"{newTankData.EngineUpgradePrices.Length - 1}";

                gunsUpgradesLabel.text = $"{newTankSaveData.gunsImprovement}/" +
                                         $"{newTankData.GunsUpgradePrices.Length - 1}";

                fuelUpgradesLabel.text = $"{newTankSaveData.fuelImprovement}/" +
                                         $"{newTankData.FuelUpgradePrices.Length - 1}";
            }

            engineUpgradePrice.text = $"${newTankData.EngineUpgradePrices[newTankSaveData.engineImprovement]}";
            gunUpgradePrice.text = $"${newTankData.GunsUpgradePrices[newTankSaveData.gunsImprovement]}";
            fuelUpgradePrice.text = $"${newTankData.FuelUpgradePrices[newTankSaveData.fuelImprovement]}";
        }
        
    }

    public void TryBuyTank()
    {
        
    }

    public void TryUpgradeCharacteristic()
    {
        
    }

}
