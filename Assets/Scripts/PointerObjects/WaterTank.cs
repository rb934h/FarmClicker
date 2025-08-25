using System;
using Enum;
using UnityEngine;

public class WaterTank : PointerObject
{
    private WaterTankState currentState = WaterTankState.NeedToRefill;
    
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
                break;
            case WaterTankState.NeedToRefill:
                ShowStateInfo("Ведро воды готово, можно забрать");
                currentState = WaterTankState.ReadyToCollect;
                IsReadyToCollect?.Invoke();
                break;
            case WaterTankState.ReadyToCollect:
                ShowStateInfo("Воду забрали, нужно наполнить снова.");
                currentState = WaterTankState.NeedToRefill;
                IsCollected?.Invoke();
                break;
            default:
                Debug.LogWarning("Unknown WaterTank state.");
                break;
        }
        
        IsAvailable = true;
    }
}
