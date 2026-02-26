using System.Linq;
using Azur.Playable.Carousel;
using Level;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace UI
{
    public class LevelSelectionMenu : MonoBehaviour
    {
        [FormerlySerializedAs("database")] public LevelDatabase _database;
    
        private Carousel _carousel;
        private IObjectResolver _objectResolver;
   
        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        private void Awake()
        {
            _carousel = GetComponent<Carousel>();
        }

        private void Start()
        {
            GenerateLevelButtons();
            _carousel?.Init();
        }

        private void GenerateLevelButtons()
        {
            var unlockedLevel = Progress.unlockedLevel;
            var levels = _database.levels;

            for (var i = levels.Length - 1; i >= 0; i--)
            {
                var level = levels[i];

                if (level.levelIndex <= unlockedLevel && level.levelButton != null)
                {
                    _objectResolver.Instantiate(level.levelButton, transform);
                }
            }

        }
    }
}
