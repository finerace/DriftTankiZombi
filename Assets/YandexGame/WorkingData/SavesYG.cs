
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

        [SerializeField] public int playerMoney;
        [SerializeField] public int playerXp;

        [SerializeField] public int trainingStage;
        
        [SerializeField] public TankSaveData[] purchasedTanks;

        [SerializeField] public LevelSaveData[] levelsOpenClose;
        
        [System.Serializable]
        public class TankSaveData
        {
            public bool isTankPurchased;

            public int engineImprovement;
            public int gunsImprovement;
            public int fuelImprovement;
        }
        
        [System.Serializable]
        public class LevelSaveData
        {
            public bool isLevelOpen;
            public int levelMaxScore;
        }

        public SavesYG()
        {
            
        }
    }
}
