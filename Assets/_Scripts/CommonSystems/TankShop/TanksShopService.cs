using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class TanksShopService : MonoBehaviour
{
    public static TanksShopService instance;
    
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

    public event Action<int> OnTankPurchased;

    private void Awake()
    {
        instance = this;
    }

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
        var targetNum = objectsDragService.TargetObjectNum;
        
        if(YandexGame.savesData.tanksData[targetNum].isTankPurchased)
            return;
        
        var currentTankData = tanksShopDatas[targetNum];

        if (playerMoneyXpService.PlayerMoney < currentTankData.Price || 
            playerMoneyXpService.PlayerDonateMoney < currentTankData.PriceDonate) return;
        
        playerMoneyXpService.PlayerMoney -= currentTankData.Price;
        playerMoneyXpService.PlayerDonateMoney -= currentTankData.PriceDonate;

        YandexGame.savesData.tanksData[targetNum].isTankPurchased = true;
        
        YandexGame.SaveProgress();
        
        OnTankPurchased?.Invoke(targetNum);
    }

    public void TryUpgradeCharacteristic(TankCharacteristics tankCharacteristics)
    {
        var targetNum = objectsDragService.TargetObjectNum;
        var currentTankData = tanksShopDatas[targetNum];
        var saveData = YandexGame.savesData;

        switch (tankCharacteristics)
        {
            case TankCharacteristics.engine:
                UpgradeEngine();
                break;
            case TankCharacteristics.gun:
                UpgradeGuns();
                break;
            case TankCharacteristics.fuel:
                UpgradeFuel();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tankCharacteristics), tankCharacteristics, null);
        }
        
        void UpgradeEngine()
        {
            var currentEngineUpgrades = saveData.tanksData[targetNum].engineImprovement;

            if (currentEngineUpgrades >= currentTankData.EngineUpgradePrices.Length)
                return;

            var upgradePrice = currentTankData.EngineUpgradePrices[currentEngineUpgrades + 1];

            if (playerMoneyXpService.PlayerMoney <= upgradePrice)
                return;

            playerMoneyXpService.PlayerMoney -= upgradePrice;
            saveData.tanksData[targetNum].engineImprovement++;
        }
        
        void UpgradeGuns()
        {
            var currentUpgrades = saveData.tanksData[targetNum].gunsImprovement;

            if (currentUpgrades >= currentTankData.GunsUpgradePrices.Length)
                return;

            var upgradePrice = currentTankData.GunsUpgradePrices[currentUpgrades + 1];

            if (playerMoneyXpService.PlayerMoney <= upgradePrice)
                return;

            playerMoneyXpService.PlayerMoney -= upgradePrice;
            saveData.tanksData[targetNum].gunsImprovement++;
        }
        
        void UpgradeFuel()
        {
            var currentUpgrades = saveData.tanksData[targetNum].fuelImprovement;

            if (currentUpgrades >= currentTankData.FuelUpgradePrices.Length)
                return;

            var upgradePrice = currentTankData.FuelUpgradePrices[currentUpgrades + 1];

            if (playerMoneyXpService.PlayerMoney <= upgradePrice)
                return;

            playerMoneyXpService.PlayerMoney -= upgradePrice;
            saveData.tanksData[targetNum].fuelImprovement++;
        }

    }
    
    public enum TankCharacteristics
    {
        engine,
        gun,
        fuel
    }
}
