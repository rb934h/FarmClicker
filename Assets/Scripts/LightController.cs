using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController
{
    private Light2D _globalLight;
    private Light2D[] _freeformLights;
    private Transform[] _freeformTransforms; // для движения света

    private Sequence _sequence;

    public LightController(Light2D globalLight, Light2D[] freeformLights)
    {
        _globalLight = globalLight;
        _freeformLights = freeformLights;

        // Получаем трансформы для движения
        _freeformTransforms = new Transform[freeformLights.Length];
        for (int i = 0; i < freeformLights.Length; i++)
        {
            _freeformTransforms[i] = freeformLights[i].transform;
        }
    }
    
    /// <summary>
    /// Заход солнца: уменьшаем intensity и двигаем свет, одновременно увеличивая радиус/фалл оф
    /// </summary>
    public void Sunset(float globalTargetIntensity, float freeformTargetIntensity, float duration,
                       float verticalOffset = -2f, float horizontalOffset = 0f)
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        // Глобальный свет
        if (_globalLight != null)
        {
            _sequence.Join(
                DOTween.To(() => _globalLight.intensity, x => _globalLight.intensity = x,
                           globalTargetIntensity, duration)
                       .SetEase(Ease.Linear)
            );
        }

        // Freeform lights
        if (_freeformLights != null)
        {
            for (int i = 0; i < _freeformLights.Length; i++)
            {
                Light2D light = _freeformLights[i];
                Transform t = _freeformTransforms[i];
                if (light == null || t == null) continue;

                // уменьшаем intensity
                _sequence.Join(
                    DOTween.To(() => light.intensity, x => light.intensity = x,
                               freeformTargetIntensity, duration/2).SetEase(Ease.Linear)
                );
                
                // перемещаем свет
                Vector3 targetPos = t.position + new Vector3(horizontalOffset, verticalOffset, 0);
                _sequence.Join(
                    t.DOMove(targetPos, duration).SetEase(Ease.InOutSine)
                );
            }
        }

        _sequence.Play();
    }

    public void Kill()
    {
        _sequence?.Kill();
    }
}
