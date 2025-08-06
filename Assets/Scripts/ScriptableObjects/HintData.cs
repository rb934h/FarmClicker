using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewHints", menuName = "Hints/HintData")]
    public class HintData : ScriptableObject
    {
        public Sprite waterSprite;
        public Sprite harvestSprite;
    }
}