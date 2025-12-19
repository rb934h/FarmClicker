using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Localization
{
    public class LanguageSwitcher : MonoBehaviour
    {
        private void Awake()
        {
            if (PlayerPrefs.HasKey("LANG"))
                SetLanguage(PlayerPrefs.GetString("LANG"));
        }

        public void SetLanguage(string localeCode)
        {
            PlayerPrefs.SetString("LANG", localeCode);
            PlayerPrefs.Save();

            StartCoroutine(SetLocaleCoroutine(localeCode));
        }

        private IEnumerator SetLocaleCoroutine(string localeCode)
        {
            yield return LocalizationSettings.InitializationOperation;

            var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);

            if (locale == null)
            {
                Debug.LogError($"Locale not found: {localeCode}");
                yield break;
            }

            LocalizationSettings.SelectedLocale = locale;
        }
    }
}