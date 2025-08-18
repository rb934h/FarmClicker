using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private PointerClickerTest _pointerClicker;
    [SerializeField] private Player _player;
    [SerializeField] private InputSystem _inputSystem;
    [SerializeField] private Image _timerImage;
    [SerializeField] private DeliveryCar _deliveryCar;
    [SerializeField] private Transform _levelGoalsUITransform;
    [SerializeField] private TMP_Text _levelGoalsText;
    
    private Timer _levelTimer;
    private LevelGoalsView _levelGoalsView;
    private Dictionary<CollectableItemData, int> _deliveredItems = new();

    private void Start()
    {
        _inputSystem.DownTouched += StartLevel;
        _levelTimer = new Timer(_timerImage);
        _levelGoalsView = new LevelGoalsView(_levelGoalsUITransform, _levelGoalsText);
        
        foreach (var levelDataGoal in _levelData.goals)
        {
            _levelGoalsView.SetGoal(levelDataGoal.itemData.name, levelDataGoal.requiredCount);
        }

    }
    
    private void StartLevel()
    {
        _inputSystem.DownTouched -= StartLevel;
        _pointerClicker.OnPointerClick += OnPointerClick;
        _player.WorkCompleted += ChangeState;
        _deliveryCar.IsDeparted += AddDeliveredItem;
     
        foreach (var screen in ScreenBase.Screens)
        {
            if(screen.GetType() == typeof(GameScreen))
            {
                screen.ShowScreen();
            }
        }
        
        _levelTimer.StartTimer(_levelData.timeToEnd);
        _levelTimer.OnTimerComplete += LevelEnd;
    }

    
    private void Update()
    {
        _levelTimer.CheckTimer();
    }

    private void OnPointerClick(Vector3 positionForInteract, PointerObject pointerObject)
    {
        if (pointerObject is Garden garden)
        {
            garden.SetSeedingData(_player.SeedingData);
            _player.InteractWithGarden(positionForInteract, garden);
        }
        else if (pointerObject is WaterTank waterTank)
        {
            _player.InteractWithWaterTank(positionForInteract, waterTank);
        }
        else if (pointerObject is DeliveryCar deliveryCar)
        {
            _player.InteractWithDeliveryCar(positionForInteract, deliveryCar);
        }
        else
        {
            Debug.LogWarning("Unknown PointerObject type clicked.");
        }
      
    }

    private void ChangeState(PointerObject pointerObject)
    {
        pointerObject.ChangeState();
    }

    private void AddDeliveredItem(List<CollectableItemData> items)
    {
        foreach (var item in items)
        {
            if (!_deliveredItems.TryAdd(item, 1))
                _deliveredItems[item]++;
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
