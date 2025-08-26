using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : BaseMenu
{
    [Header("Buttons")] 
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _exitButton;
    
    [Header("Menu")]
    [SerializeField] private PauseMenu _pauseMenu;
    
    void Start()
    {
        _resumeButton.onClick.AddListener(ShowPauseMenu);
        _exitButton.onClick.AddListener(Toggle);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsOpen)
            Toggle();
    }

    private void ShowPauseMenu()
    {
        Hide();
        _pauseMenu.Show();
        
    }
}