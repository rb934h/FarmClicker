using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


namespace DefaultNamespace
{
    public class Animal : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _minBounds;
        [SerializeField] private Vector2 _maxBounds;
        
        public event Action<Vector2> OnReachedTarget;
        
        private void Start()
        {
            Wander();
        }
        
        
        public void GoTo(Vector2 target)
        {
            var distance = Vector2.Distance(transform.position, target);
            var duration = distance / _speed;

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

        private void Wander()
        {
            Vector2 randomTarget = new Vector2(
                Random.Range(_minBounds.x, _maxBounds.x),
                Random.Range(_minBounds.y, _maxBounds.y)
            );

            var distance = Vector2.Distance(transform.position, randomTarget);
            var duration = distance / _speed;

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