using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using PointerObjects;
using ScriptableObjects;
using UI;
using UnityEngine;
using VContainer;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private PointerInteractor _pointerClicker;
    [SerializeField] private WeatherManager _weatherManager;
    [SerializeField] private TutorialManager _tutorialManager;
    [SerializeField] private TimeOfDayManager _timeOfDayManager;

    private Dictionary<CollectableItemData, int> _deliveredItems = new();
    private Player _player;
    private Chest _chest;
    private InputSystem _inputSystem;
    private Timer _levelTimer;
    private GameScreen _gameScreen;

    public LevelData levelData => _levelData;

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
            if (screen is GameScreen gameScreen)
            {
                _gameScreen = gameScreen;
            }
        }

        _pointerClicker.PointerClicked += OnPointerClick;
        _chest.IsSolded += AddDeliveredItem;
        _levelTimer.OnTimerComplete += Lose;
        _levelTimer.EveningArrived += OnEveningArrived;
    }
    
    private void Awake()
    {
        foreach (var pointerObject in _pointerClicker.PointerObjects)
        {
            if (pointerObject is Chest chest)
                _chest = chest;
        }
    }

    private async void Start()
    {
        _weatherManager.SetWeather(_levelData.weatherTypes);
        _timeOfDayManager.EnableDayMode();

        await _gameScreen.ShowConvert(_levelData.convertMessage, _levelData.convertMessageSender);
        _inputSystem.DownTouched += StartLevel;
    }

    private async void StartLevel()
    {
        _inputSystem.DownTouched -= StartLevel;
        await _gameScreen.HideConvert();
        OnConvertHided();
    }

    private void Update()
    {
        _levelTimer.CheckTimer();
    }

    private void OnConvertHided()
    {
        _gameScreen.SetAvailableItems(_levelData.collectableItems);
        _gameScreen.SetLevelGoals(_levelData.goals, _levelData.requiredCoins);
        _gameScreen.ShowScreen();

        _tutorialManager?.StartTutorial();

        _levelTimer.StartTimer(_levelData.timeToEnd);
        _timeOfDayManager.Sunset(0.5f, 0f, _levelData.timeToEnd, -15f, 20f);
    }

    private void OnEveningArrived()
    {
        _levelTimer.EveningArrived -= OnEveningArrived;
        _timeOfDayManager.EnableNightMode();
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
        if (_player.inventory.coins < _levelData.requiredCoins)
        {
            return;
        }

        foreach (var goal in _levelData.goals)
        {
            _deliveredItems.TryGetValue(goal.itemData, out var deliveredCount);

            if (deliveredCount < goal.requiredCount)
            {
                return;
            }
        }
        
        Win();
    }

    private async void Win()
    {
        await _gameScreen.ShowConvert(_levelData.convertWinMessage, _levelData.convertMessageSender);
        UnlockNextLevel();
        _inputSystem.DownTouched += LevelEnd;
    }

    private async void Lose()
    {
        await _gameScreen.ShowConvert(_levelData.convertLoseMessage, _levelData.convertMessageSender);
        _inputSystem.DownTouched += LevelEnd;
    }

    private void LevelEnd()
    {
        _inputSystem.DownTouched -= LevelEnd;
        CrossSceneAnimation.Instance
            .PlayTransition(SceneLoader.LoadMainMenuScene)
            .Forget();;
    }

    private void UnlockNextLevel()
    {
        if (Progress.unlockedLevel <= _levelData.levelIndex)
        {
            Progress.unlockedLevel = _levelData.levelIndex + 1;
        }
    }

    private void OnDisable()
    {
        _pointerClicker.PointerClicked -= OnPointerClick;
        _chest.IsSolded -= AddDeliveredItem;
        //_levelTimer.OnTimerComplete -= LevelEnd;
    }
}