using Level;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] private int levelIndex;
        
        private Button _button;
        
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(SelectAndStartLevel);
            _button.gameObject.SetActive(levelIndex <= Progress.unlockedLevel);
        }

        private void SelectAndStartLevel()
        {
            _button.interactable = false;
            LevelSession.selectedLevelIndex = levelIndex;
            CrossSceneAnimation.Instance
                .PlayTransition(SceneLoader.LoadLevelScene)
                .Forget();
        }
    }
} 