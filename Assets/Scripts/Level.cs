using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private PointerClickerTest _pointerClicker;
    [SerializeField] private Player _player;
    [SerializeField] private InputSystem _inputSystem;
    
    private void Start()
    {
        _inputSystem.DownTouched += StartLevel;
    }

    private void StartLevel()
    {
        _inputSystem.DownTouched -= StartLevel;
        _pointerClicker.OnPointerClick += OnPointerClick;
        _player.InteractEnded += ChangeState;
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
        if (pointerObject is Garden garden)
        {
            garden.ChangeState();
        }
        else if (pointerObject is WaterTank waterTank)
        {
            waterTank.ChangeState();
        }
        else if (pointerObject is DeliveryCar deliveryCar)
        {
            deliveryCar.ChangeState();
        }
        else
        {
            Debug.LogWarning("Unknown PointerObject type clicked.");
        }
        
    }
   
}
