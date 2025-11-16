using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private CrossSceneAnimation crossSceneAnimation;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectAndStartLevel);
    }

    private void SelectAndStartLevel()
    {
        _button.interactable = false;
        LevelSession.selectedLevelIndex = levelIndex;
        crossSceneAnimation.Play(SceneLoader.LoadLevelScene);
    }
}