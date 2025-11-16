using System;
using System.Collections.Generic;
using DG.Tweening;
using PointerObjects;
using ScriptableObjects;
using UI;
using UnityEngine;
using VContainer;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private PointerInteractor _pointerClicker;
    [SerializeField] private LightController _lightController;
    [SerializeField] private WeatherManager _weatherManager;
    [SerializeField] private TutorialManager _tutorialManager;
    
    private Dictionary<CollectableItemData, int> _deliveredItems = new();
    private Player _player;
    private Chest _chest;
    private InputSystem _inputSystem;
    private Timer _levelTimer;
    private GameScreen _gameScreen;
    
    private readonly float _delayBeforeLevelStart = 3f;
   
    [Inject]
    public void Construct(Player player, InputSystem inputSystem, Timer levelTimer)
    {
        _player = player;
        _inputSystem = inputSystem;
        _levelTimer = levelTimer;
    }

    private void OnEnable()
    {
        foreach (var screen in ScreenBase.Screens)
        {
            if(screen is GameScreen gameScreen)
            {
                _gameScreen = gameScreen;
            }
        }
        
        _pointerClicker.PointerClicked += OnPointerClick;
        _chest.IsSolded += AddDeliveredItem;
        _levelTimer.OnTimerComplete += LevelEnd;
        _gameScreen.ConvertHided += OnConvertHided;
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
        
        _gameScreen.ShowConvert(_levelData.convertMessage, _levelData.convertMessageSender);
        
        DOVirtual.DelayedCall(_delayBeforeLevelStart, () =>
        {
            _inputSystem.DownTouched += StartLevel;
        });
    }
    
    private void StartLevel()
    {
        _inputSystem.DownTouched -= StartLevel;
        
        _gameScreen.HideConvert();
    }
    
    private void Update()
    {
        _levelTimer.CheckTimer();
    }

    private void OnConvertHided()
    {
        _gameScreen.ConvertHided -= OnConvertHided;
        
        _gameScreen.SetAvailableItems(_levelData.collectableItems);
        _gameScreen.SetLevelGoals(_levelData.goals);
        _gameScreen.ShowScreen();
        
        _tutorialManager?.StartTutorial();
        
        _levelTimer.StartTimer(_levelData.timeToEnd);
        _lightController.Sunset(0.5f, 0f, _levelData.timeToEnd, -5f,10f);
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
        CrossSceneAnimation.Instance.Play(SceneLoader.LoadMainMenuScene);
    }

    private void OnDisable()
    {
        _pointerClicker.PointerClicked -= OnPointerClick;
        _chest.IsSolded -= AddDeliveredItem;
        _levelTimer.OnTimerComplete -= LevelEnd;
        _gameScreen.ConvertHided -= OnConvertHided;
    }
}
