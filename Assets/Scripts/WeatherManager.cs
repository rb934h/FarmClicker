using UnityEngine;
using System.Collections.Generic;
using Enum;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private List<WeatherEffectSO> weatherEffects;

    private Dictionary<WeatherType, GameObject> _effectInstances = new Dictionary<WeatherType, GameObject>();
    private GameObject _currentEffect;
    private Vector3 _effectPosition = new (-5,10,0);

    public void SetWeather(WeatherType type)
    {
        if (_currentEffect != null)
            _currentEffect.SetActive(false);

        if (type == WeatherType.None)
        {
            _currentEffect = null;
            return;
        }

        var effectData = weatherEffects.Find(e => e.type == type);
        if (effectData == null)
        {
            Debug.LogWarning($"Effect for {type} not found!");
            return;
        }

        if (!_effectInstances.ContainsKey(type))
        {
            var instance = Instantiate(effectData.prefab, transform);
            instance.transform.position = _effectPosition;
            _effectInstances[type] = instance;
        }

        _currentEffect = _effectInstances[type];
        _currentEffect.SetActive(true);
        
        // if (effectData.sound != null)
        //     AudioSource.PlayClipAtPoint(effectData.sound, Camera.main!.transform.position);
    }
}