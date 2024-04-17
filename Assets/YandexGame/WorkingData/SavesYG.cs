
using UnityEngine;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        [SerializeField] public int playerMoney = 99999;
        [SerializeField] public int playerDonateMoney = 9999;
        [SerializeField] public int playerXp;

        [SerializeField] public int selectedTankNum = 0;
        
        [SerializeField] public int trainingStage;

        [SerializeField] public float masterVolume;
        [SerializeField] public float musicVolume;
        [SerializeField] public bool mobileManageAlternativeMenu; 
        
        [SerializeField] public TankSaveData[] tanksData = new TankSaveData[16];

        [SerializeField] public LevelSaveData[] levelsData = new LevelSaveData[16];
        
        [System.Serializable]
        public class TankSaveData
        {
            public bool isTankPurchased;

            public int engineImprovement = 1;
            public int gunsImprovement = 1;
            public int fuelImprovement = 1;
        }
        
        [System.Serializable]
        public class LevelSaveData
        {
            public bool isLevelCompleted;
            public bool isLevelUnlocked;
            public int levelHighScore;
        }

        public SavesYG()
        {
            tanksData = new SavesYG.TankSaveData[16];
            levelsData = new SavesYG.LevelSaveData[16];

            for (var i = 0; i < tanksData.Length; i++)
            {
                tanksData[i] ??= new SavesYG.TankSaveData();
            }

            for (var i = 0; i < levelsData.Length; i++)
            {
                levelsData[i] ??= new SavesYG.LevelSaveData();
            }

            tanksData[0].isTankPurchased = true;
        }
    }
}
