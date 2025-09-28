using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewAnimal", menuName = "Animal/AnimalData")]
    public class AnimalData : ScriptableObject
    {
        public Sprite _babySprite;
        public Sprite _adultSprite;
        public Sprite _withWoolSprite;
        public float _speed;
    }
}