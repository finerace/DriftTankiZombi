using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class TanksShopService : NumObserveReference,IObserveNum
{
    public static TanksShopService instance;
    
    [SerializeField] private ShopData[] tanksShopDatas;
    [SerializeField] private ShopLockData[] shopLockData;
    [SerializeField] private ObjectsDragServiceReference objectsDragService;
    
    private PlayerMoneyXpService playerMoneyXpService;
    
    [Space] 
    
    [SerializeField] private TMP_Text tankName;

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
    
    [SerializeField] private float charUiBarImprovementPower = 0.01f;

    private ShopData selectedTankShopData;
    
    private float charManage; 
    private float charGun;
    private float charEngine; 
    private float charFuel;

    public event Action<int> OnTankPurchased;

    private void OnEnable()
    {
        var selectedTankNum = YandexGame.savesData.selectedTankNum;

        objectsDragService.InstantDragToObject(selectedTankNum);
        
        UpdateUI();
    }

    private void OnDisable()
    {
        objectsDragService.InstantDragToObject(selectedTankShopData.ID-1);
    }

    private void Awake()
    {
        instance = this;

        selectedTankShopData = tanksShopDatas[0];
        
        YandexGame.GetDataEvent += InitSavedSelectedTank;
        void InitSavedSelectedTank()
        {
            var selectedTankNum = YandexGame.savesData.selectedTankNum;

            selectedTankShopData = tanksShopDatas[selectedTankNum];
            objectsDragService.InstantDragToObject(selectedTankNum);

            UpdateUI();
            
            shopLockData = FindObjectOfType<ShopLockDatas>().ShopLockDatass;
            UpdateLockTanks();
            void UpdateLockTanks()
            {
                for (int i = 0; i < tanksShopDatas.Length; i++)
                {
                    if (YandexGame.savesData.tanksData[i].isTankPurchased)
                    {
                        shopLockData[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        shopLockData[i].MoneyPrice.text = tanksShopDatas[i].Price.ToShortenInt();

                        if (tanksShopDatas[i].PriceDonate != 0)
                            shopLockData[i].DonateMoneyPrice.text = tanksShopDatas[i].Price.ToShortenInt();
                        else
                            shopLockData[i].DonationMoneyPrice.gameObject.SetActive(false);
                    }
                }
            }
        }
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
            var savedData = YandexGame.savesData.tanksData[currentNum];
                
                charManage = newTankData.ManageChar + savedData.engineImprovement * charUiBarImprovementPower;
                charGun = newTankData.GunChar + savedData.gunsImprovement * charUiBarImprovementPower;
                charEngine = newTankData.SpeedChar + savedData.engineImprovement * charUiBarImprovementPower;
                charFuel = newTankData.FuelChar + savedData.fuelImprovement * charUiBarImprovementPower;
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
        shopLockData[targetNum].gameObject.SetActive(false);
        
        OnTankPurchased?.Invoke(targetNum);
    }

    public void TryUpgradeCharacteristic(int tankCharacteristics)
    {
        var targetNum = objectsDragService.TargetObjectNum;
        var currentTankData = tanksShopDatas[targetNum];
        var saveData = YandexGame.savesData;
        
        if(!saveData.tanksData[targetNum].isTankPurchased)
            return;
            
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

    public float GetNum(int id)
    {
        switch (id)
        {
            case 1:
                return charManage;
            case 2:
                return charGun;
            case 3:
                return charEngine;
            case 4:
                return charFuel;
        }

        return 0;
    }

    public (float min, float max) GetBarParam(int id)
    {
        return (0f, 1f);
    }

    public event Action OnBarParamChange;
    public event Action<int, int> OnObserveNumChange;
}
