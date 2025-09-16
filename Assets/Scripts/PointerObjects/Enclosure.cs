using System;
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

        [HideInInspector]
        public bool HasWater;
        [HideInInspector]
        public bool HasFood;

        private void Start()
        {
            _workTime = 1f;
        }

        public void SetWaterToBowl()
        {
            _bowlForWater.sprite = _bowlSpritesData.waterBowl;
            HasWater = true;
        }
        
        public void SetFoodToBowl()
        {
            _bowlForFood.sprite = _bowlSpritesData.foodBowl;
            HasFood = true;
        }
        
        
    }
}