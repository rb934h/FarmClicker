using System.Collections.Generic;
using Enum;
using ScriptableObjects;
using UnityEngine;

namespace Level
{
    public class WeatherManager : MonoBehaviour
    {
        [SerializeField] private WeatherData weatherData;

        private Dictionary<WeatherType, GameObject> _effectInstances = new Dictionary<WeatherType, GameObject>();
        private GameObject _currentEffect;

        public void SetWeather(WeatherType[] types)
        {
            if (_currentEffect != null)
                _currentEffect.SetActive(false);

            foreach (var kvp in _effectInstances)
                kvp.Value.SetActive(false);
       
            if (types == null || types.Length == 0 || (types.Length == 1 && types[0] == WeatherType.None))
            {
                _currentEffect = null;
                return;
            }

            GameObject lastEffect = null;

            foreach (var type in types)
            {
                if (type == WeatherType.None)
                    continue;

                var effectData = weatherData.weatherEffects.Find(e => e.type == type);
                if (effectData == null)
                {
                    Debug.LogWarning($"Effect for {type} not found!");
                    continue;
                }

                if (!_effectInstances.ContainsKey(type))
                {
                    var instance = Instantiate(effectData.prefab, transform);
                    _effectInstances[type] = instance;
                }

                var instanceObj = _effectInstances[type];
                instanceObj.SetActive(true);
                lastEffect = instanceObj;

                // Если нужен звук — можно активировать тут по каждому
                // if (effectData.sound != null)
                //     AudioSource.PlayClipAtPoint(effectData.sound, Camera.main!.transform.position);
            }
       
            _currentEffect = lastEffect;
        }

    }
}