using System;
using System.Collections;
using System.Collections.Generic;
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

        [HideInInspector] public bool HasWater;
        [HideInInspector] public bool HasFood;
        private bool _foodInUse = false;
        private bool _waterInUse = false;
        private float _checkAnimalsDelay = 5f;
        
        private Queue<Animal> _foodQueue = new Queue<Animal>();
        private Queue<Animal> _waterQueue = new Queue<Animal>();

        private void Start()
        {
            _workTime = 1f;

            foreach (var animal in _animals)
            {
                animal.OnReachedTarget += OnAnimalReached;
                animal.OnReachedTarget += (_) => animal.TryGrowUp();
            }
            
            StartCoroutine(CheckAnimalsRoutine());
        }

        private IEnumerator CheckAnimalsRoutine()
        {
            while (true)
            {
                if (HasFood)
                {
                    if (_foodQueue.Count == 0)
                    {
                        foreach (var animal in _animals)
                        {
                            if (animal.NeedFood)
                                _foodQueue.Enqueue(animal);
                        }
                    }

                    TrySendNextFoodAnimal();
                }

                if (HasWater)
                {
                    if (_waterQueue.Count == 0)
                    {
                        foreach (var animal in _animals)
                        {
                            if (animal.NeedWater)
                                _waterQueue.Enqueue(animal);
                        }
                    }

                    TrySendNextWaterAnimal();
                }

                yield return _checkAnimalsDelay;
            }
        }

        private void CleanBowl(Vector2 position)
        {
            if ((Vector2)_bowlForFood.transform.position == position)
            {
                _bowlForFood.sprite = _bowlSpritesData.emptyBowl;
                HasFood = false;
            }
            else
            {
                _bowlForWater.sprite = _bowlSpritesData.emptyBowl;
                HasWater = false;
            }
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

        private void TrySendNextFoodAnimal()
        {
            if (!HasFood || _foodQueue.Count == 0 || _foodInUse)
                return;

            var animal = _foodQueue.Dequeue();
            _foodInUse = true;
            animal.GoTo(_bowlForFood.transform.position);
            animal.SetFood();
        }

        private void TrySendNextWaterAnimal()
        {
            if (!HasWater || _waterQueue.Count == 0 || _waterInUse)
                return;

            var animal = _waterQueue.Dequeue();
            _waterInUse = true; 
            animal.GoTo(_bowlForWater.transform.position);
            animal.SetWater();
        }


        private void OnAnimalReached(Vector2 position)
        {
            if ((Vector2)_bowlForFood.transform.position == position)
            {
                _foodInUse = false;
                CleanBowl(position);
                TrySendNextFoodAnimal();
            }
            else if ((Vector2)_bowlForWater.transform.position == position)
            {
                _waterInUse = false;
                CleanBowl(position);
                TrySendNextWaterAnimal();
            }
        }
    }
}
