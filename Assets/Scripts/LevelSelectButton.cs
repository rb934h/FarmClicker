using UnityEngine;
using UnityEngine.SceneManagement;
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
        crossSceneAnimation.Play(LoadLevel);
    }

    private void LoadLevel()
    {
        LevelSession.selectedLevelIndex = levelIndex;
        SceneManager.LoadSceneAsync(1);
    }
}