using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Azur.Playable.Carousel
{
    public class CarouselAnimator
    {
        public event Action SequenceUpdated;
        public event Action SequenceCompleted;
        public event Action SequenceKilled;

        private readonly ItemDataAccess _itemDataAccess;
        private readonly CarouselUIService _carouselUIService;
        private readonly CarouselAnimatorConfig _carouselAnimatorConfig;

        private Sequence _animationSequence;
        private Tween _rollbackTween;

        private float _idleTime;
        private float _currentDuration;

        public CarouselAnimator(ItemDataAccess itemDataAccess, CarouselUIService uiService,
            CarouselAnimatorConfig carouselAnimatorConfig)
        {
            _itemDataAccess = itemDataAccess;
            _carouselUIService = uiService;
            _carouselAnimatorConfig = carouselAnimatorConfig;

            DoTweenInitialize();
            SetScale();
        }

        public void PlayBackward(float moveDuration)
        {
            _itemDataAccess.Reverse();
            _animationSequence.PlayBackwards();
            _animationSequence.OnStepComplete(() => _animationSequence.Kill());

            PlayForward(moveDuration, (Ease)_carouselAnimatorConfig.EaseType);
        }

        public void PlayAutoScroll(float deltaTime)
        {
            if (_carouselAnimatorConfig.AutoScroll == false)
            {
                return;
            }

            _idleTime += deltaTime;

            if (_idleTime >= _carouselAnimatorConfig.IdleThreshold == false)
            {
                return;
            }

            _animationSequence?.Kill();

            PlayForward(_carouselAnimatorConfig.AutoScrollMoveDuration, (Ease)_carouselAnimatorConfig.EaseType);
            _idleTime -= _carouselAnimatorConfig.AutoScrollMoveDuration;
        }

        public void PlayForward(float moveDuration, Ease easeType = Ease.Linear)
        {
            AnimateScaleAndMove(moveDuration, easeType);
        }

        public void Rollback()
        {
            if (!_animationSequence.IsActive() ||
                _animationSequence.ElapsedPercentage() >= _carouselAnimatorConfig.ReturnThreshold)
            {
                _animationSequence.Play();
                return;
            }

            _rollbackTween = DOVirtual.DelayedCall(
                _carouselAnimatorConfig.ReturnDelay,
                () =>
                {
                    _animationSequence.PlayBackwards();
                    _animationSequence.OnStepComplete(() => _animationSequence.Kill());
                });
        }

        public void Pause() => _animationSequence.TogglePause();
        public void Play() => _animationSequence.Play();
        public void ResetIdleTime() => _idleTime = 0f;

        private static void DoTweenInitialize()
        {
            DOTween.SetTweensCapacity(400, 20);
            DOTween.Init(false, true);
        }

        private void AnimateScaleAndMove(float moveDuration, Ease easeType)
        {
            if (_rollbackTween != null)
            {
                _rollbackTween.Kill();
                _animationSequence?.Play();
            }

            if (_animationSequence != null
                && _animationSequence.IsActive()
                && _animationSequence.IsPlaying()
                && _currentDuration <= moveDuration
                && _animationSequence.ElapsedPercentage() >= _carouselAnimatorConfig.ReturnThreshold)
            {
                _currentDuration = moveDuration * 20;
                var timeScaleDuration = 4f;
                _animationSequence
                    .DOTimeScale(_currentDuration, timeScaleDuration);
                _currentDuration = 0;
            }

            if (_animationSequence != null && _animationSequence.IsActive())
            {
                return;
            }

            _animationSequence = DOTween.Sequence();

            for (var i = 0; i < _itemDataAccess.items.Count; i++)
            {
                var item = _itemDataAccess.items[i];
                var targetPosition = (i == _itemDataAccess.items.Count - 1)
                    ? _itemDataAccess.items[0].position
                    : _itemDataAccess.items[i + 1].position;

                var scaleFactor = 1f / (1f + _itemDataAccess.ItemsDepth * targetPosition.z * 0.001f);
                var targetScale = _itemDataAccess.ItemScale * scaleFactor;

                Tween moveTween = item
                    .DOMove(targetPosition, moveDuration)
                    .SetEase(easeType)
                    .SetUpdate(UpdateType.Normal, true)
                    .OnUpdate(() => SequenceUpdated?.Invoke());

                _animationSequence.Join(moveTween);

                if (_carouselUIService.IsCanvas == false) continue;
                Tween scaleTween = item
                    .DOScale(targetScale, moveDuration)
                    .SetEase(easeType);

                _animationSequence.Join(scaleTween);
            }

            _animationSequence.Play()
                .OnComplete(() => SequenceCompleted?.Invoke())
                .OnKill(() => SequenceKilled?.Invoke());
        }

        private void SetScale()
        {
            if (_carouselUIService.IsCanvas == false || Application.isPlaying == false)
            {
                return;
            }

            var sortedItems = _itemDataAccess.items
                .OrderByDescending(item => item.position.z)
                .ToList();

            var currentItem = sortedItems.Last();

            foreach (var item in sortedItems)
            {
                var scaleFactor = item == currentItem
                    ? 1f
                    : 1f / (1f + _itemDataAccess.ItemsDepth * item.position.z * 0.001f);
                var targetScale = _itemDataAccess.ItemScale * scaleFactor;

                item.localScale = targetScale;
            }
        }
    }
}