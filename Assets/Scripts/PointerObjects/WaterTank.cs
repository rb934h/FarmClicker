using System;
using Enum;
using UnityEngine;
using VContainer;

public class WaterTank : PointerObject
{
    private WaterTankState currentState = WaterTankState.Empty;
    
    public WaterTankState State => currentState;
    
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
        switch (currentState)
        { 
            case WaterTankState.Empty:
                ShowStateInfo("Ведро воды готово, можно забрать");
                currentState = WaterTankState.ReadyToCollect;
                IsReadyToCollect?.Invoke();
                break;
            case WaterTankState.ReadyToCollect:
                if(_playerInventory.hasWater)
                    return;
                ShowStateInfo("Воду забрали, нужно наполнить снова.");
                currentState = WaterTankState.Empty;
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
