using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class UIBounceEffect : MonoBehaviour
    {
        [Header("Настройки движения")] 
        [SerializeField] private float rotationAngle = 2f;
        [SerializeField] private float rotationDuration = 1.5f;

        [Header("Настройки масштаба")] 
        [SerializeField] private float scaleAmount = 0.05f;
        [SerializeField] private float scaleDuration = 1.2f;

        private Sequence sequence;

        private void OnEnable()
        {
            PlayEffect();
        }

        private void OnDisable()
        {
            sequence?.Kill();
        }

        private void PlayEffect()
        {
            sequence = DOTween.Sequence().SetUpdate(true);

            sequence.Append(transform.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration)
                .SetEase(Ease.InOutSine));

            sequence.Append(transform.DORotate(new Vector3(0, 0, -rotationAngle), rotationDuration)
                .SetEase(Ease.InOutSine));

            sequence.SetLoops(-1, LoopType.Yoyo);

            transform.DOScale(transform.localScale * (1f + scaleAmount), scaleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}