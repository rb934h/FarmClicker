using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DefaultNamespace
{
    public class URPVolume : MonoBehaviour
    {
        private Volume _volume;
        private ChromaticAberration _chromaticAberration;
        private DepthOfField _depthOfField;
        
        private void Start()
        {
            _volume = GetComponent<Volume>();
            
            _volume.profile.TryGet(out _chromaticAberration);
            _volume.profile.TryGet(out _depthOfField);
        }

        public void ChangeChromaticAberrationValue(float duration) =>
            TweenFloatParameter(_chromaticAberration.intensity, 0f, 1f, duration);

        public void ChangeDepthOfFieldValue(float duration) =>
            TweenFloatParameter(_depthOfField.focalLength, 1f, 150f, duration);
        
        private void TweenFloatParameter(FloatParameter parameter, float from, float to, float duration)
        {
            if (parameter == null) return;
            
            float target = Mathf.Approximately(parameter.value, to) ? from : to;
            
            DOTween.To(
                () => parameter.value,
                x => parameter.value = x,
                target,
                duration
            ).SetEase(Ease.InOutSine)
                .SetUpdate(true);;
        }
    }
}