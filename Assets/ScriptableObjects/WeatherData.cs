using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Weather/Weather Data", fileName = "NewWeatherData")]
    public class WeatherData : ScriptableObject
    {
        public List<WeatherEffectSO> weatherEffects;
    }
}