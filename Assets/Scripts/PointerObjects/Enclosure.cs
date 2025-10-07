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
        
        public Animal[] Animals => _animals;

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
                            if (animal.NeedFood && !animal.IsGoingToTarget)
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
                            if (animal.NeedWater && !animal.IsGoingToTarget)
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
        private void TrySendNextFoodAnimal()
        {
            if (!HasFood || _foodInUse || _foodQueue.Count == 0)
                return;

            Animal animal = null;

            while (_foodQueue.Count > 0)
            {
                var next = _foodQueue.Dequeue();
                if (!next.IsGoingToTarget)
                {
                    animal = next;
                    break;
                }
            }

            if (animal == null)
                return;

            _foodInUse = true;
            animal.GoTo(_bowlForFood.transform.position);
            animal.SetFood();
        }
        private void TrySendNextWaterAnimal()
        {
            if (!HasWater || _waterInUse || _waterQueue.Count == 0)
                return;

            Animal animal = null;

            while (_waterQueue.Count > 0)
            {
                var next = _waterQueue.Dequeue();
                if (!next.IsGoingToTarget)
                {
                    animal = next;
                    break;
                }
            }

            if (animal == null)
                return;

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
