using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;

namespace Level
{
    public class TimeOfDayManager : MonoBehaviour
    {
        [SerializeField] private Light2D _globalLight;
        [SerializeField] private Light2D[] _dayLights;
        [SerializeField] private NightData _nightData;
        [SerializeField] private Light2D[] _nightLights;

        private AudioPlayer _audioPlayer;
        private Sequence _sequence;

        [Inject]
        public void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        public void EnableNightMode()
        {
            _audioPlayer.PlayNightSounds();

            foreach (var nightLight in _nightLights)
            {
                DOTween.To(
                    () => nightLight.intensity,
                    value => nightLight.intensity = value,
                    _nightData.lightIntensity,
                    _nightData.lightDuration
                );
            }

            Instantiate(_nightData.particleSystem);
        }

        public void EnableDayMode()
        {
            _audioPlayer.PlayDaySounds();
        }

        public void Sunset(float globalTargetIntensity, float freeformTargetIntensity, float duration,
            float verticalOffset = -2f, float horizontalOffset = 0f)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            if (_globalLight != null)
            {
                _sequence.Join(
                    DOTween.To(() => _globalLight.intensity, x => _globalLight.intensity = x,
                            globalTargetIntensity, duration)
                        .SetEase(Ease.Linear)
                );
            }

            if (_dayLights != null)
            {
                foreach (var light2D in _dayLights)
                {
                    var lightTransform = light2D.transform;
                    if (light2D == null) continue;

                    _sequence.Join(
                        DOTween.To(() => light2D.intensity, x => light2D.intensity = x,
                            freeformTargetIntensity, duration / 2).SetEase(Ease.Linear)
                    );

                    var targetPos = lightTransform.position + new Vector3(horizontalOffset, verticalOffset, 0);
                    _sequence.Join(
                        lightTransform.DOMove(targetPos, duration).SetEase(Ease.InOutSine)
                    );
                }
            }

            _sequence.Play();
        }
    }
}