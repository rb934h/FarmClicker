using System;
using DG.Tweening;
using Enum;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;


namespace DefaultNamespace
{
    public class Animal : MonoBehaviour
    {
        [SerializeField] protected AnimalData _animalData;
        [SerializeField] private Vector2 _minBounds;
        [SerializeField] private Vector2 _maxBounds;
        
        protected SpriteRenderer _spriteRenderer;
        protected Animator _animator;
        private bool _needFood;
        private bool _needWater;
        
        public AnimalGrowStates _currentGrowState;
            
        public event Action<Vector2> OnReachedTarget;
        
        public bool NeedFood => _needFood;
        public bool NeedWater => _needWater;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            _needFood = true;
            _needWater = true;
            
            Wander();
        }
        
        public void GoTo(Vector2 target)
        {
            var distance = Vector2.Distance(transform.position, target);
            var duration = distance / _animalData._speed;

            transform
                .DOKill();
            transform
                .DOMove(target, duration).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Wander();
                    OnReachedTarget?.Invoke(target);
                });

            FlipSprite(target);
        }

        public void SetFood()
        {
            _needFood = false;
        }
        
        public void SetWater()
        {
            _needWater = false;
        }
        
        public virtual void TryGrowUp()
        {
            if(_needFood || _needWater)
                return;
            
            if (_currentGrowState == AnimalGrowStates.Adult)
                DoSomethingWhenAdult();
            
            _currentGrowState = AnimalGrowStates.Adult;
            
            _spriteRenderer.sprite = _currentGrowState == AnimalGrowStates.Baby ? _animalData._babySprite : _animalData._adultSprite;
            _animator.Play(_currentGrowState == AnimalGrowStates.Baby ? "BabySheepRun" : "AdultSheepRun");

            _needFood = true;
            _needWater = true;
        }

        protected virtual void DoSomethingWhenAdult() { }

        private void Wander()
        {
            Vector2 randomTarget = new Vector2(
                Random.Range(_minBounds.x, _maxBounds.x),
                Random.Range(_minBounds.y, _maxBounds.y)
            );

            var distance = Vector2.Distance(transform.position, randomTarget);
            var duration = distance / _animalData._speed;

            transform
                .DOKill();
            transform
                .DOMove(randomTarget, duration)
                .SetEase(Ease.Linear)
                .OnComplete(Wander);
        
            FlipSprite(randomTarget);
        }

        private void FlipSprite(Vector2 target)
        {
            if (_spriteRenderer == null) return;

            if (target.x > transform.position.x)
                _spriteRenderer.flipX = true;
            else if (target.x < transform.position.x)
                _spriteRenderer.flipX = false;
        }
    }
}