using System.Collections.Generic;
using DefaultNamespace;
using PointerObjects;
using ScriptableObjects;
using UnityEngine;
using VContainer;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private PointerInteractor _pointerClicker;
    [SerializeField] private Timer _levelTimer;
    [SerializeField] private LightController _lightController;
    [SerializeField] private WeatherManager _weatherManager;
    
    private Dictionary<CollectableItemData, int> _deliveredItems = new();
    private Player _player;
    private Chest _chest;
    private InputSystem _inputSystem;
   
    [Inject]
    public void Construct(Player player, InputSystem inputSystem)
    {
        _player = player;
        _inputSystem = inputSystem;
    }

    private void Awake()
    {
        foreach (var pointerObject in _pointerClicker.PointerObjects)
        {
            if(pointerObject is Chest chest)
                _chest = chest;
        }
    }

    private void Start()
    {
        _weatherManager.SetWeather(_levelData.weatherType);
        _inputSystem.DownTouched += StartLevel;
    }
    
    private void StartLevel()
    {
        _inputSystem.DownTouched -= StartLevel;
        _pointerClicker.OnPointerClick += OnPointerClick;
        _chest.IsSolded += AddDeliveredItem;
        
        foreach (var screen in ScreenBase.Screens)
        {
            if(screen is GameScreen gameScreen)
            {
                gameScreen.SetLevelGoals(_levelData.goals);
                gameScreen.ShowScreen();
            }
        }
        
        _levelTimer.StartTimer(_levelData.timeToEnd);
        _lightController.Sunset(0.5f, 0f, _levelData.timeToEnd, -5f,10f);
        _levelTimer.OnTimerComplete += LevelEnd;
    }

    
    private void Update()
    {
        _levelTimer.CheckTimer();
    }

    private void OnPointerClick(Vector2 positionForInteract, PointerObject pointerObject)
    {
        if (pointerObject is Garden garden && _player.inventory.currentSeed != null)
        {
            garden.SetSeed(_player.inventory.currentSeed); // TODO Hmm..
        }
        
        _player.InteractWithPointerObject(positionForInteract, pointerObject);
        _player.inventory.currentSeed = null;
    }

    private void AddDeliveredItem(List<CollectableItemData> items)
    {
        foreach (var item in items)
        {
            if (!_deliveredItems.TryAdd(item, 1))
            {
                _deliveredItems[item]++;
                
            }
            
            _player.inventory.AddCoins(item.price);
        }
       
        CheckLevelGoals();
    }

    private void CheckLevelGoals()
    {
        foreach (var goal in _levelData.goals)
        {
            _deliveredItems.TryGetValue(goal.itemData, out var deliveredCount);
            
            if (deliveredCount < goal.requiredCount)
            {
                return;
            }
        }

        LevelEnd();
    }
    
    private void LevelEnd()
    {
        foreach (var screen in ScreenBase.Screens)
        {
            if(screen.GetType() == typeof(LevelEndScreen))
            {
                screen.ShowScreen();
            }
            else
            {
                screen.HideScreen();
            }
        }
    }
}
