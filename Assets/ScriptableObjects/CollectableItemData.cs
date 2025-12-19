using UnityEngine;
using UnityEngine.Localization;

namespace ScriptableObjects
{
    public abstract class CollectableItemData : ScriptableObject
    {
        public LocalizedString itemName;
        public int price;
        public float growTime;
        public float ruinTime;
        public Sprite spriteForHands;
        public Sprite[] spritesForGarden;
    }
}