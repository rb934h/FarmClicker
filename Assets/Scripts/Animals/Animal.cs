using System;
using DG.Tweening;
using Enum;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animals
{
    public class Animal : MonoBehaviour
    {
        [SerializeField] protected AnimalData _animalData;
        [SerializeField] private Transform _wanderArea;

        private static readonly int GrowUp = Animator.StringToHash("GrowUp");
        protected static readonly int SpecialPerk = Animator.StringToHash("SpecialPerk");

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

        public virtual CollectableItemData GetSpecialItem()
        {
            ChangeGeneration(AnimalGrowStates.Adult);

            return _animalData._specialItemData;
        }

        private void ChangeGeneration(AnimalGrowStates generation)
        {
            _currentGrowState = generation;
            _needFood = true;
            _needWater = true;
        }

        public void TryGrowUp()
        {
            if (_needFood || _needWater || _currentGrowState == AnimalGrowStates.Special)
                return;

            if (_currentGrowState == AnimalGrowStates.Adult)
            {
                DoSomethingWhenAdult();
                return;
            }

            ChangeGeneration(AnimalGrowStates.Adult);

            _spriteRenderer.sprite = _animalData._adultSprite;
            _animator.SetBool(GrowUp, true);
        }

        protected virtual void DoSomethingWhenAdult()
        {
        }

        private void Wander()
        {
            var randomTarget = GetRandomPointInArea(_wanderArea);

            var distance = Vector2.Distance(transform.position, randomTarget);
            var duration = distance / _animalData._speed;

            transform.DOKill();
            transform.DOMove(randomTarget, duration)
                .SetEase(Ease.Linear)
                .OnComplete(Wander);

            FlipSprite(randomTarget);
        }

        private Vector2 GetRandomPointInArea(Transform area)
        {
            var center = area.position;
            var scale = area.localScale * 0.5f;

            return new Vector2(
                Random.Range(center.x - scale.x, center.x + scale.x),
                Random.Range(center.y - scale.y, center.y + scale.y)
            );
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