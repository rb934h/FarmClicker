using Enum;
using UnityEngine;

public class WaterTank : PointerObject
{
    private WaterTankState currentState = WaterTankState.Empty;
    
    public WaterTankState State => currentState;
   
    public override void ChangeState()
    {
        switch (currentState)
        {
            case WaterTankState.Empty:
                Debug.Log("WaterTank is empty. Refill needed.");
                currentState = WaterTankState.NeedToRefill;
                break;
            case WaterTankState.NeedToRefill:
                Debug.Log("Refilling WaterTank...");
                currentState = WaterTankState.ReadyToCollect;
                break;
            case WaterTankState.ReadyToCollect:
                Debug.Log("Water is ready to collect.");
                currentState = WaterTankState.Empty;
                break;
            default:
                Debug.LogWarning("Unknown WaterTank state.");
                break;
        }
    }
}
