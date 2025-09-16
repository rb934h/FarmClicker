using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace Scripts
{
    public static class HintAnimator
    {
        private static readonly Dictionary<SpriteRenderer, Sequence> _sequences = new();

        public static void Show(SpriteRenderer hintSprite)
        {
            StopSequence(hintSprite);

            Sequence seq = DOTween.Sequence()
                .Append(hintSprite.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack));

            _sequences[hintSprite] = seq;
        }

        public static void Show(SpriteRenderer hintSprite, float interval)
        {
            StopSequence(hintSprite);

            Sequence seq = DOTween.Sequence()
                .Append(hintSprite.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack))
                .AppendInterval(interval)
                .Append(hintSprite.transform.DOScale(0f, 0.25f).SetEase(Ease.InBack));

            _sequences[hintSprite] = seq;
        }

        public static void Hide(SpriteRenderer hintSprite, bool clearSprite = false, float duration = 0.25f)
        {
            StopSequence(hintSprite);

            Sequence seq = DOTween.Sequence()
                .Append(hintSprite.transform.DOScale(0f, duration).SetEase(Ease.OutBack));

            if (clearSprite)
                seq.OnComplete(() => hintSprite.sprite = null);

            _sequences[hintSprite] = seq;
        }

        private static void StopSequence(SpriteRenderer hintSprite)
        {
            if (_sequences.TryGetValue(hintSprite, out Sequence seq) && seq.IsActive())
            {
                seq.Kill(true);
                _sequences.Remove(hintSprite);
            }
        }
    }
}