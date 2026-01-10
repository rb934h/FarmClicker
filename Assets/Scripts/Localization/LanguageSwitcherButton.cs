using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Localization
{
    [RequireComponent(typeof(Button))]
    public class LanguageSwitcherButton : MonoBehaviour
    {
        private LanguageSwitcher _languageSwitcher;

        private void Awake()
        {
            _languageSwitcher = GetComponent<LanguageSwitcher>();

            var button = GetComponent<Button>();
            button.onClick.AddListener(ToggleLanguage);
        }

        private void ToggleLanguage()
        {
            var currentCode = LocalizationSettings.SelectedLocale.Identifier.Code;
            var nextLocale = currentCode == "en" ? "ru-RU" : "en";

            _languageSwitcher.SetLanguage(nextLocale);
        }
    }
}