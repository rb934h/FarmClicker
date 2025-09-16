using System;
using DefaultNamespace;
using ScriptableObjects.Data.Enclosure;
using UnityEngine;

namespace PointerObjects
{
    public class Enclosure : PointerObject
    {
        [Header("Bowl settings")] 
        [SerializeField] private SpriteRenderer _bowlForFood;
        [SerializeField] private SpriteRenderer _bowlForWater;
        [SerializeField] private BowlSpritesData _bowlSpritesData;

        [Header("Animals settings")] 
        [SerializeField] private Animal[] _animals;

        [HideInInspector]
        public bool HasWater;
        [HideInInspector]
        public bool HasFood;

        private void Start()
        {
            _workTime = 1f;

            foreach (var animal in _animals)
            {
                animal.OnReachedTarget += CleanBowl;
            }
        }

        private void CleanBowl(Vector2 position)
        {
            if ((Vector2)_bowlForFood.transform.position == position)
                _bowlForFood.sprite = _bowlSpritesData.emptyBowl;
            else
                _bowlForWater.sprite = _bowlSpritesData.emptyBowl;
        }

        public void SetWaterToBowl()
        {
            _bowlForWater.sprite = _bowlSpritesData.waterBowl;
            HasWater = true;
            
            foreach (var animal in _animals)
            {
                animal.GoTo(_bowlForWater.transform.position);
            }
        }
        
        public void SetFoodToBowl()
        {
            _bowlForFood.sprite = _bowlSpritesData.foodBowl;
            HasFood = true;
            
            foreach (var animal in _animals)
            {
                animal.GoTo(_bowlForFood.transform.position);
            }
        }
        
        
    }
}