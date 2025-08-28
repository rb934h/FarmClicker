using System;
using Enum;
using UnityEngine;
using VContainer;

public class WaterTank : PointerObject
{
    public WaterTankState State = WaterTankState.Empty;
    
    public event Action IsReadyToCollect; 
    public event Action IsCollected; 
    
    private PlayerInventoryData _playerInventory;

    [Inject]
    public void Construct(PlayerInventoryData playerInventoryData)
    {
        _playerInventory = playerInventoryData;
    }
   
    public override void ChangeState()
    {
        switch (State)
        { 
            case WaterTankState.Empty:
                // ShowStateInfo("Ведро воды готово, можно забрать");
                // State = WaterTankState.ReadyToCollect;
                IsReadyToCollect?.Invoke();
                break;
            case WaterTankState.ReadyToCollect:
                if(_playerInventory.hasWater)
                    return;
                ShowStateInfo("Воду забрали, нужно наполнить снова.");
                State = WaterTankState.Empty;
                _playerInventory.FillWater();
                OnPlayerAnimationStateChanged(PlayerAnimationState.PlayerWatering);
                IsCollected?.Invoke();
                break;
            default:
                Debug.LogWarning("Unknown WaterTank state.");
                break;
        }
        
        IsAvailable = true;
    }
}
