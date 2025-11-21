using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    private CrossSceneAnimation _crossSceneAnimation;
    //[SerializeField] private AudioSource _clickSound;

    private Button _button;

    [Inject]
    private void Construct(CrossSceneAnimation crossSceneAnimation)
    {
        _crossSceneAnimation = crossSceneAnimation;
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectAndStartLevel);
        _button.gameObject.SetActive(levelIndex <= Progress.unlockedLevel);
    }

    private void SelectAndStartLevel()
    {
       // _clickSound.Play();
            
        _button.interactable = false;
        LevelSession.selectedLevelIndex = levelIndex;
        _crossSceneAnimation.Play(SceneLoader.LoadLevelScene);
    }
} 