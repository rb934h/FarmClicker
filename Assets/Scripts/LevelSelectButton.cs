using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private CrossSceneAnimation crossSceneAnimation;
    [SerializeField] private AudioSource _clickSound;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectAndStartLevel);
    }

    private void SelectAndStartLevel()
    {
        _clickSound.Play();
            
        _button.interactable = false;
        LevelSession.selectedLevelIndex = levelIndex;
        crossSceneAnimation.Play(SceneLoader.LoadLevelScene);
    }
}