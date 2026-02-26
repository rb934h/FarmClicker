using Level;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class LevelSelectButton : MonoBehaviour
    {
        [FormerlySerializedAs("levelIndex")] [SerializeField] private int _levelIndex;
        
        private Button _button;
        
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(SelectAndStartLevel);
            _button.gameObject.SetActive(_levelIndex <= Progress.unlockedLevel);
        }

        private void SelectAndStartLevel()
        {
            _button.interactable = false;
            LevelSession.SelectedLevelIndex = _levelIndex;
            CrossSceneAnimation.Instance
                .PlayTransition(SceneLoader.LoadLevelScene)
                .Forget();
        }
    }
}
