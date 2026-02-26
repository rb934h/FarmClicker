using System;
using DG.Tweening;
using Enum;
using PointerObjects;

namespace Strategies.WaterTankStrategy
{
    public class WaterTankFIllStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player.Player player, PointerObject pointerObject)
        {
            if (pointerObject is not WaterTank waterTank) return false;
            if (waterTank._state is not WaterTankState.Empty) return false;
            DOVirtual.DelayedCall(waterTank.WorkTime, waterTank.ChangeTile);
            waterTank._state = WaterTankState.ReadyToCollect;
            OnComplete?.Invoke();
            return true;
        }
    }
}
