using System;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace DefaultNamespace
{
    public class FillBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _fillSprite;
        [SerializeField] private float _showScale = 0.5f;
        [SerializeField] private float _showDuration = 0.25f;
        [SerializeField] private Color _fillColor = Color.green;
        [SerializeField] private Color _emptyingColor = Color.red;

        private Sequence _fillSequence;
        private Vector3 _initialScale;

        public event Action OnFill;
        public event Action OnEmpty;

        private void Awake()
        {
            _initialScale = _fillSprite.transform.localScale;
        }

        public void Filling(float time)
        {
            _fillSequence?.Kill();

            var scale = _initialScale;
            scale.y = 0;
            _fillSprite.transform.localScale = scale;

            _fillSequence = DOTween.Sequence()
                .Append(_fillSprite.DOColor(_fillColor, time))
                .Join(_fillSprite.transform.DOScaleY(_initialScale.y, time)).SetEase(Ease.Linear)
                .OnComplete(() => OnFill?.Invoke());
        }

        public void Emptying(float time)
        {
            _fillSequence?.Kill();

            _fillSequence = DOTween.Sequence()
                .Append(_fillSprite.DOColor(_emptyingColor, time))
                .Join(_fillSprite.transform.DOScaleY(0, time)).SetEase(Ease.Linear)
                .OnComplete(() => OnEmpty?.Invoke());
        }

        public void Show()
        {
            transform.DOScale(Vector3.one * _showScale, _showDuration)
                .SetEase(Ease.OutBack);
        }

        public void Hide()
        {
            _fillSequence?.Kill();
            
            transform.DOScale(Vector3.zero, _showDuration)
                .SetEase(Ease.Linear);
        }
    }
}