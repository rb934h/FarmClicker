using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Animals;
using ScriptableObjects.Data.Enclosure;
using UnityEngine;
using UnityEngine.Serialization;

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
        private Queue<Animal> _foodQueue = new();
        private Queue<Animal> _waterQueue = new();

        public Animal[] Animals => _animals;

        private void Start()
        {
            _workTime = 1f;

            foreach (var animal in _animals)
            {
                animal.OnReachedTarget += OnAnimalReached;
                animal.OnReachedTarget += (_) => animal.TryGrowUp();
            }

            CheckAnimalsLoop().Forget();
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
        
        private async UniTaskVoid CheckAnimalsLoop()
        {
            var token = this.GetCancellationTokenOnDestroy();

            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    
                    if (HasFood)
                    {
                        if (_foodQueue.Count == 0)
                        {
                            foreach (var a in _animals)
                            {
                                if (a.NeedFood && !a.IsGoingToTarget)
                                    _foodQueue.Enqueue(a);
                            }
                        }

                        TrySendNextFoodAnimal();
                    }
                    
                    if (HasWater)
                    {
                        if (_waterQueue.Count == 0)
                        {
                            foreach (var a in _animals)
                            {
                                if (a.NeedWater && !a.IsGoingToTarget)
                                    _waterQueue.Enqueue(a);
                            }
                        }

                        TrySendNextWaterAnimal();
                    }

                    await UniTask.WaitForSeconds(_checkAnimalsDelay, cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Token was cancelled");
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
                if (next.IsGoingToTarget == false)
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
