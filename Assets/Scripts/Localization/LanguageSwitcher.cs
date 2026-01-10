using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Localization
{
    public class LanguageSwitcher : MonoBehaviour
    {
        private void Awake()
        {
            if (PlayerPrefs.HasKey("LANG"))
            {
                SetLanguage(PlayerPrefs.GetString("LANG"));
            }
        }

        public void SetLanguage(string localeCode)
        {
            PlayerPrefs.SetString("LANG", localeCode);
            PlayerPrefs.Save();

            SetLocaleAsync(localeCode).Forget();
        }

        private async UniTask SetLocaleAsync(string localeCode)
        {
            await LocalizationSettings.InitializationOperation.ToUniTask();

            var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);

            if (locale == null)
            {
                Debug.LogError($"Locale not found: {localeCode}");
                return;
            }

            LocalizationSettings.SelectedLocale = locale;
        }
    }
}