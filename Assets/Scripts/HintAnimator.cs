using UnityEngine;
using DG.Tweening;

namespace Scripts
{
    public static class HintAnimator
    {
        private static Sequence _currentSequence;

        public static void Show(SpriteRenderer hintSprite)
        {
            StopSequence();

            _currentSequence = DOTween.Sequence()
                .Append(hintSprite.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack));
        }

        public static void Show(SpriteRenderer hintSprite, float interval)
        {
            StopSequence();

            _currentSequence = DOTween.Sequence()
                .Append(hintSprite.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack))
                .AppendInterval(interval)
                .Append(hintSprite.transform.DOScale(0f, 0.25f).SetEase(Ease.InBack));
        }

        public static void Hide(SpriteRenderer hintSprite, bool clearSprite = false)
        {
            StopSequence();

            _currentSequence = DOTween.Sequence()
                .Append(hintSprite.transform.DOScale(0f, 0.25f).SetEase(Ease.OutBack));

            if (clearSprite)
                _currentSequence.OnComplete(() => hintSprite.sprite = null);
        }

        private static void StopSequence()
        {
            if (_currentSequence != null && _currentSequence.IsActive())
            {
                _currentSequence.Kill(true);
            }
        }
    }
}