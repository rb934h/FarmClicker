using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Localization
{
    [RequireComponent(typeof(Button))]
    public class LanguageSwitcherButton : MonoBehaviour
    {
        [Header("Icons")]
        [SerializeField] private Sprite enIcon;
        [SerializeField] private Sprite ruIcon;

        private LanguageSwitcher _languageSwitcher;
        private Image _buttonImage;

        private void Awake()
        {
            _languageSwitcher = GetComponent<LanguageSwitcher>();

            var button = GetComponent<Button>();
            button.onClick.AddListener(ToggleLanguage);

            _buttonImage = button.image;
        }

        private void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += UpdateIcon;
            UpdateIcon(LocalizationSettings.SelectedLocale);
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= UpdateIcon;
        }

        private void ToggleLanguage()
        {
            var currentCode = LocalizationSettings.SelectedLocale.Identifier.Code;
            var nextLocale = currentCode == "en" ? "ru-RU" : "en";

            _languageSwitcher.SetLanguage(nextLocale);
        }

        private void UpdateIcon(Locale locale)
        {
            if (_buttonImage == null || locale == null)
                return;

            var code = locale.Identifier.Code;
            _buttonImage.sprite = code.StartsWith("en") ? enIcon : ruIcon;
        }
    }
}