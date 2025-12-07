using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    [Header("Buttons")] 
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    
    private SettingsMenu _settingsMenu;

    private void Start()
    {
        foreach (var menu in BaseMenus)
        {
            if (menu is SettingsMenu settings)
            {
                _settingsMenu = settings;
                break;
            }
        }
        
        _resumeButton.onClick.AddListener(Toggle);
        _exitButton.onClick.AddListener(Exit);
        _settingsButton.onClick.AddListener(ShowSettingsMenu);
    }
  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        } 
    }
    
    private void Exit()
    {
        Hide();
        CrossSceneAnimation.Instance
            .PlayTransition(SceneLoader.LoadMainMenuScene)
            .Forget();
    }
    
    private void ShowSettingsMenu()
    {
        SwitchTo(_settingsMenu);
    }

}