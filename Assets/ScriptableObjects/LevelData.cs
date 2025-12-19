using System.Collections.Generic;
using Enum;
using UI;
using UnityEngine;
using UnityEngine.Localization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Levels/LevelData")]
    public class LevelData : ScriptableObject
    {
        [Header("Info")]
        public int levelIndex;
        public float timeToEnd;

        [Header("Goals")]
        public List<LevelGoal> goals;
        public int requiredCoins;
        
        [Header("Convert")]
        public LocalizedString convertMessage;
        public LocalizedString convertWinMessage;
        public LocalizedString convertLoseMessage;
        public LocalizedString convertMessageSender;
        
        [Header("Weather")]
        public WeatherType[] weatherTypes;
        
        [Space]
        [Header("Available items")]
        public List<CollectableItemData> collectableItems;
        
        [Header("UI")]
        public LevelSelectButton levelButton;
    }

    [System.Serializable]
    public class LevelGoal
    {
        public CollectableItemData itemData;
        public int requiredCount;
    }
}