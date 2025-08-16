using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Levels/LevelData")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Info")]
        public string levelName;
        public float timeToEnd;

        [Header("Level goals")]
        public List<LevelGoal> goals;
        public int requiredCoins;
    }

    [System.Serializable]
    public class LevelGoal
    {
        public CollectableItemData itemData;
        public int requiredCount;
    }
}