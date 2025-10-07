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
        
        private static readonly int GrowUp = Animator.StringToHash("GrowUp");
        private static readonly int SpecialPerk = Animator.StringToHash("SpecialPerk");
      
        private bool _needFood;
        private bool _needWater;
        
        protected SpriteRenderer _spriteRenderer;
        protected Animator _animator;
        
        public AnimalGrowStates _currentGrowState;
        public bool IsGoingToTarget { get; private set; }
            
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
            IsGoingToTarget = true;
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
                    IsGoingToTarget = false;
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

        public CollectableItemData GetSpecialItem()
        {
            _spriteRenderer.sprite = _animalData._adultSprite;
            _animator.SetBool(SpecialPerk, false);
            
            return _animalData._specialItemData;
        }
        public virtual void TryGrowUp()
        {
            if(_needFood || _needWater || _currentGrowState == AnimalGrowStates.Special)
                return;
            
            if (_currentGrowState == AnimalGrowStates.Adult)
            {
                DoSomethingWhenAdult();
                return;
            }
            
            _currentGrowState = AnimalGrowStates.Adult;
            
            _spriteRenderer.sprite = _animalData._adultSprite;
            _animator.SetBool(GrowUp, true);

            _needFood = true;
            _needWater = true;
        }

        protected virtual void DoSomethingWhenAdult()
        {
            _animator.SetBool(SpecialPerk, true);
            _currentGrowState = AnimalGrowStates.Special;
        }

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