using DG.Tweening;
using Enum;
using PointerObjects;

namespace Strategies.WaterTankStrategy
{
    public class WaterTankFIllStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not WaterTank waterTank) return false;
            if (waterTank.State is not WaterTankState.Empty) return false;
            DOVirtual.DelayedCall(waterTank.WorkTime, waterTank.ChangeTile);
            waterTank.State = WaterTankState.ReadyToCollect;
            return true;
        }
    }
}