using UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : BaseMenu
{
    [Header("Buttons")] 
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _exitButton;
    
    private PauseMenu _pauseMenu;
    
    void Start()
    {
        foreach (var menu in BaseMenus)
        {
            if (menu is PauseMenu pause)
            {
                _pauseMenu = pause;
                break;
            }
        }
      
        _resumeButton.onClick.AddListener(ShowPauseMenu);
        _exitButton.onClick.AddListener(Toggle);    
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsOpen)
        {
            ShowPauseMenu();
        } 
    }

    private void ShowPauseMenu()
    {
        SwitchTo(_pauseMenu);
    }

}