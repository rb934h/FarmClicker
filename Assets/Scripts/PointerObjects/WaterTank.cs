using Enum;
using UnityEngine;

public class WaterTank : PointerObject
{
    private WaterTankState currentState = WaterTankState.NeedToRefill;
    
    public WaterTankState State => currentState;
   
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
                ShowStateInfo("Идет наполнение...");
                currentState = WaterTankState.ReadyToCollect;
                break;
            case WaterTankState.ReadyToCollect:
                ShowStateInfo("Воду забрали, нужно наполнить снова.");
                currentState = WaterTankState.NeedToRefill;
                break;
            default:
                Debug.LogWarning("Unknown WaterTank state.");
                break;
        }
        
        IsAvailable = true;
    }
}
