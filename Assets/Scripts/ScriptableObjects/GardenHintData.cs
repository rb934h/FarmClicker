using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewHints", menuName = "Hints/GardenHintData")]
    public class GardenHintData : ScriptableObject
    {
        public Sprite waterSprite;
        public Sprite harvestSprite;
    }
}