using Enum;
using UnityEngine;

[CreateAssetMenu(menuName = "Weather/Weather Effect", fileName = "NewWeatherEffect")]
public class WeatherEffectSO : ScriptableObject
{
    public WeatherType type;
    public GameObject prefab;
    public AudioClip sound;
}