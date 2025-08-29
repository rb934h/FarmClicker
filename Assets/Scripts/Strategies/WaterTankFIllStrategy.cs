using Enum;
using UnityEngine;

namespace Strategies
{
    public class WaterTankFIllStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not WaterTank waterTank) return false;
            if (waterTank.State is not WaterTankState.Empty) return false;
            waterTank.ChangeTile();
            waterTank.State = WaterTankState.ReadyToCollect;
            return true;
        }
    }
}