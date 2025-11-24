using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LevelRunner : MonoBehaviour
{
    [SerializeField] private Level[] _levels;

    private IObjectResolver _objectResolver;
    private Level _currentLevelInstance;
    private int _currentLevelIndex;

    [Inject]
    public void Construct(IObjectResolver objectResolver)
    {
        _objectResolver = objectResolver;
    }
    private void Start()
    {
        StartLevel(LevelSession.selectedLevelIndex);
    }
    private void StartLevel(int index)
    {
        if (_currentLevelInstance != null)
                    Destroy(_currentLevelInstance);
        
  
        foreach (var level in _levels)
        {
            if (level.LevelData.levelIndex == index)
            {
                _currentLevelInstance = _objectResolver.Instantiate(level, Vector3.zero, Quaternion.identity);
                _currentLevelIndex = index;
                return;
            }
        }
    }

    public void RestartLevel() => StartLevel(_currentLevelIndex);
    public void NextLevel() => StartLevel(_currentLevelIndex + 1);
}
