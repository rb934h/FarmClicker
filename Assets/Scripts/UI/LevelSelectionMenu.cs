using Azur.Playable.Carousel;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UI
{
    public class LevelSelectionMenu : MonoBehaviour
    {
        public LevelDatabase database; 
    
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
        
            foreach (var level in database.levels)
            {
                if (level.levelIndex <= unlockedLevel && level.levelButton != null)
                {
                    _objectResolver.Instantiate(level.levelButton, transform);
                }
            }
        }
    }
}