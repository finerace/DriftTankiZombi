using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class TanksShopService : MonoBehaviour
{
    public static TanksShopService instance;
    
    [SerializeField] private ShopData[] tanksShopDatas;
    [SerializeField] private ObjectsDragServiceReference objectsDragService;
    
    private PlayerMoneyXpService playerMoneyXpService;
    
    [Space] 
    
    [SerializeField] private TMP_Text tankName;

    [SerializeField] private Image charManage;
    [SerializeField] private Image charGun;
    [SerializeField] private Image charEngine;
    [SerializeField] private Image charFuel;
    
    [Space]
    
    [SerializeField] private TMP_Text engineUpgradesLabel;
    [SerializeField] private TMP_Text gunsUpgradesLabel;
    [SerializeField] private TMP_Text fuelUpgradesLabel;

    [Space]
    
    [SerializeField] private TMP_Text engineUpgradePrice;
    [SerializeField] private TMP_Text gunUpgradePrice;
    [SerializeField] private TMP_Text fuelUpgradePrice;

    [Space] 
    
    [SerializeField] private TMP_Text buyOrSelectTankButtonLabel;

    private ShopData selectedTankShopData;

    public event Action<int> OnTankPurchased;

    private void OnEnable()
    {
        var selectedTankNum = YandexGame.savesData.selectedTankNum;

        objectsDragService.InstantDragToObject(selectedTankNum);
        
        UpdateUI();
    }

    private void Awake()
    {
        instance = this;

        var selectedTankNum = YandexGame.savesData.selectedTankNum;
        
        selectedTankShopData = tanksShopDatas[selectedTankNum];
        objectsDragService.InstantDragToObject(selectedTankNum);
        
        UpdateUI();
    }

    private void Start()
    {
        playerMoneyXpService = FindObjectOfType<PlayerMoneyXpService>();
        
        if(objectsDragService != null)
            objectsDragService.OnTargetObjectNumChange += UpdateUI;
    }

    private void UpdateUI()
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

        SetUpgradePrices();
        void SetUpgradePrices()
            {
                engineUpgradePrice.text = newTankData.EngineUpgradePrices.Length-2 >= newTankSaveData.engineImprovement ? 
                    $"${newTankData.EngineUpgradePrices[newTankSaveData.engineImprovement]}" :
                    CurrentLanguageData.GetText(52);
                
                gunUpgradePrice.text = newTankData.GunsUpgradePrices.Length-2 >= newTankSaveData.gunsImprovement ? 
                    $"${newTankData.GunsUpgradePrices[newTankSaveData.gunsImprovement]}" :
                    CurrentLanguageData.GetText(52);                
                
                fuelUpgradePrice.text = newTankData.FuelUpgradePrices.Length-2 >= newTankSaveData.fuelImprovement ? 
                    $"${newTankData.FuelUpgradePrices[newTankSaveData.fuelImprovement]}" :
                    CurrentLanguageData.GetText(52);
            }

        SetBuySelectTankLabel();
        void SetBuySelectTankLabel()
            {
                var isTankPurchased = newTankSaveData.isTankPurchased;
                var isTankSelected = newTankData.ID == selectedTankShopData.ID;

                buyOrSelectTankButtonLabel.text = isTankPurchased switch
                {
                    true when isTankSelected => CurrentLanguageData.GetText(51),
                    true => CurrentLanguageData.GetText(50),
                    _ => CurrentLanguageData.GetText(49)
                };
            }
    }
    
    public void TryBuyTank()
    {
        var targetNum = objectsDragService.TargetObjectNum;
        var currentTankData = tanksShopDatas[targetNum];

        if(YandexGame.savesData.tanksData[targetNum].isTankPurchased)
        {
            selectedTankShopData = currentTankData;
            UpdateUI();

            YandexGame.savesData.selectedTankNum = targetNum;
            return;
        }

        if (playerMoneyXpService.PlayerMoney < currentTankData.Price || 
            playerMoneyXpService.PlayerDonateMoney < currentTankData.PriceDonate) return;
        
        playerMoneyXpService.PlayerMoney -= currentTankData.Price;
        playerMoneyXpService.PlayerDonateMoney -= currentTankData.PriceDonate;

        YandexGame.savesData.tanksData[targetNum].isTankPurchased = true;
        YandexGame.SaveProgress();
        
        UpdateUI();

        OnTankPurchased?.Invoke(targetNum);
    }

    public void TryUpgradeCharacteristic(int tankCharacteristics)
    {
        var targetNum = objectsDragService.TargetObjectNum;
        var currentTankData = tanksShopDatas[targetNum];
        var saveData = YandexGame.savesData;

        switch ((TankCharacteristics)tankCharacteristics)
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

            if (currentEngineUpgrades >= currentTankData.EngineUpgradePrices.Length-1)
                return;

            var upgradePrice = currentTankData.EngineUpgradePrices[currentEngineUpgrades];

            if (playerMoneyXpService.PlayerMoney <= upgradePrice)
                return;

            playerMoneyXpService.PlayerMoney -= upgradePrice;
            saveData.tanksData[targetNum].engineImprovement++;
        }
        
        void UpgradeGuns()
        {
            var currentUpgrades = saveData.tanksData[targetNum].gunsImprovement;

            if (currentUpgrades >= currentTankData.GunsUpgradePrices.Length-1)
                return;

            var upgradePrice = currentTankData.GunsUpgradePrices[currentUpgrades];

            if (playerMoneyXpService.PlayerMoney <= upgradePrice)
                return;

            playerMoneyXpService.PlayerMoney -= upgradePrice;
            saveData.tanksData[targetNum].gunsImprovement++;
        }
        
        void UpgradeFuel()
        {
            var currentUpgrades = saveData.tanksData[targetNum].fuelImprovement;

            if (currentUpgrades >= currentTankData.FuelUpgradePrices.Length-1)
                return;

            var upgradePrice = currentTankData.FuelUpgradePrices[currentUpgrades];

            if (playerMoneyXpService.PlayerMoney <= upgradePrice)
                return;

            playerMoneyXpService.PlayerMoney -= upgradePrice;
            saveData.tanksData[targetNum].fuelImprovement++;
        }

        UpdateUI();
    }

    public (ShopData tankShopData,SavesYG.TankSaveData tankSaveData) GetCurrentTankData()
    {
        return (selectedTankShopData,
            YandexGame.savesData.tanksData[objectsDragService.TargetObjectNum]);
    }
    
    public enum TankCharacteristics
    {
        engine,
        gun,
        fuel
    }
}
