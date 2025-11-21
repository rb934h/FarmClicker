using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewAnimal", menuName = "Animal/AnimalData")]
    public class AnimalData : ScriptableObject
    {
        public Sprite _babySprite;
        public Sprite _adultSprite;
        public Sprite _specialStateSprite;
        public SpecialItemData _specialItemData;
        public float _speed;
        [Space]
        public AnimalHints _hints;
    }

    [Serializable]
    public class AnimalHints
    {
        public Sprite _hungryEmotionSprite;
        public Sprite _thirstyEmotionSprite;
    }
}