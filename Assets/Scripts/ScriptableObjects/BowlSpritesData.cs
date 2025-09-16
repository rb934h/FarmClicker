using UnityEngine;

namespace ScriptableObjects.Data.Enclosure
{
    [CreateAssetMenu(fileName = "NewBowlSprites", menuName = "Enclosure/BowlSpritesData")]
    
    public class BowlSpritesData : ScriptableObject
    {
        public Sprite emptyBowl;
        public Sprite foodBowl;
        public Sprite waterBowl;
    }
}