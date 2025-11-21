using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NightConfig", menuName = "DayNightConfigs/Night")]
    public class NightData : ScriptableObject
    {
        public float lightDuration;
        public float lightIntensity;
        public ParticleSystem particleSystem;
    }
}