using UnityEngine;

namespace Level
{
    public static class Progress
    {
        public static int unlockedLevel
        {
            get => PlayerPrefs.GetInt("UnlockedLevel", 0);
            set => PlayerPrefs.SetInt("UnlockedLevel", value);
        }
    }
}