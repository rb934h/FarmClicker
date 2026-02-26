using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class UIBounceEffect : MonoBehaviour
    {
        [Header("Настройки движения")] 
        [FormerlySerializedAs("rotationAngle")] [SerializeField] private float _rotationAngle = 2f;
        [FormerlySerializedAs("rotationDuration")] [SerializeField] private float _rotationDuration = 1.5f;

        [Header("Настройки масштаба")] 
        [FormerlySerializedAs("scaleAmount")] [SerializeField] private float _scaleAmount = 0.05f;
        [FormerlySerializedAs("scaleDuration")] [SerializeField] private float _scaleDuration = 1.2f;

        private Sequence _sequence;
        private Tween _scaleTween;

        private void OnEnable()
        {
            PlayEffect();
        }

        private void OnDisable()
        {
            KillTweens();
        }

        private void OnDestroy()
        {
            KillTweens();
        }

        private void KillTweens()
        {
            // Безопасно убиваем все анимации на объекте
            _sequence?.Kill();
            _scaleTween?.Kill();
        }

        private void PlayEffect()
        {
            // Если объект уничтожен или null, не запускаем анимацию
            if (transform == null) return;

            _sequence = DOTween.Sequence().SetUpdate(true);

            _sequence.Append(transform.DORotate(new Vector3(0, 0, _rotationAngle), _rotationDuration)
                .SetEase(Ease.InOutSine));

            _sequence.Append(transform.DORotate(new Vector3(0, 0, -_rotationAngle), _rotationDuration)
                .SetEase(Ease.InOutSine));

            _sequence.SetLoops(-1, LoopType.Yoyo);

            _scaleTween = transform.DOScale(transform.localScale * (1f + _scaleAmount), _scaleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
