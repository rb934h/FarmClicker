using System;
using DG.Tweening;
using Enum;
using PointerObjects;

namespace Strategies.WaterTankStrategy
{
    public class WaterTankTakeStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player.Player player, PointerObject pointerObject)
        {
            if (pointerObject is not WaterTank waterTank) return false;
            if (waterTank._state is not WaterTankState.ReadyToCollect || player.inventory.hasWater || !player.inventory.canAddItem) return false;
            player.animator.PlayAnimation(PlayerAnimationState.PlayerWateringReverse);
            DOVirtual.DelayedCall(waterTank.WorkTime, () =>
            {
                player.inventory.FillWater();
                waterTank.ChangeTile();
                waterTank._state = WaterTankState.Empty;
            });
            OnComplete?.Invoke();
            return true;
        }
    }
}
