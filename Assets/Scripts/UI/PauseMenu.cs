using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    [Header("Buttons")] 
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    [Header("Menu")]
    [SerializeField] private SettingsMenu _settingsMenu;

    private void Start()
    {
        _resumeButton.onClick.AddListener(Toggle);
        _exitButton.onClick.AddListener(Exit);
        _settingsButton.onClick.AddListener(ShowSettingsMenu);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_settingsMenu.IsOpen)
            Toggle();
    }
    private void Exit()
    {
        SceneManager.LoadScene(0);
    }

    private void ShowSettingsMenu()
    {
        _settingsMenu.Show();
        Hide();
    }
}