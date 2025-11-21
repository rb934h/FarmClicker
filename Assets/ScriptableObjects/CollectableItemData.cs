using UnityEngine;

namespace ScriptableObjects
{
    public abstract class CollectableItemData : ScriptableObject
    {
        public string itemName;
        public int price;
        public float growTime;
        public float ruinTime;
        public Sprite spriteForHands;
        public Sprite[] spritesForGarden;
    }
}