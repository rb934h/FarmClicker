using System.Collections.Generic;
using Enum;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Levels/LevelData")]
    public class LevelData : ScriptableObject
    {
        [Header("Info")]
        public string levelName;
        public float timeToEnd;

        [Header("Goals")]
        public List<LevelGoal> goals;
        public int requiredCoins;
        
        [Header("Weather")]
        public WeatherType weatherType;
    }

    [System.Serializable]
    public class LevelGoal
    {
        public CollectableItemData itemData;
        public int requiredCount;
    }
}