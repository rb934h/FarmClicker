using System;
using Enum;
using UnityEngine;

public class WaterTank : PointerObject
{
    private WaterTankState currentState = WaterTankState.Empty;
    
    public WaterTankState State => currentState;
    
    public event Action IsReadyToCollect; 
    public event Action IsCollected; 
   
    public override void ChangeState()
    {
        if(!IsAvailable)
            return;
            
        IsAvailable = false;
        
        switch (currentState)
        { 
            case WaterTankState.Empty:
                ShowStateInfo("Ведро воды готово, можно забрать");
                currentState = WaterTankState.ReadyToCollect;
                IsReadyToCollect?.Invoke();
                break;
            case WaterTankState.ReadyToCollect:
                ShowStateInfo("Воду забрали, нужно наполнить снова.");
                currentState = WaterTankState.Empty;
                IsCollected?.Invoke();
                break;
            default:
                Debug.LogWarning("Unknown WaterTank state.");
                break;
        }
        
        IsAvailable = true;
    }
}
