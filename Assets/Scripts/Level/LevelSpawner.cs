using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Level
{
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField] private global::Level.Level[] _levels;

        private IObjectResolver _objectResolver;
        private global::Level.Level _currentLevelInstance;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
    
        private void Start()
        {
            Spawn(LevelSession.selectedLevelIndex);
        }
    
        private void Spawn(int index)
        {
            if (_currentLevelInstance != null)
                Destroy(_currentLevelInstance);
        
            foreach (var level in _levels)
            {
                if (level.levelData.levelIndex == index)
                {
                    _currentLevelInstance = _objectResolver.Instantiate(level, Vector3.zero, Quaternion.identity);
                    return;
                }
            }
        }
    }
}
