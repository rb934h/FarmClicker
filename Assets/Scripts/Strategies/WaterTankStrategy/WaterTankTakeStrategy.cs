using Enum;
using PointerObjects;

namespace Strategies.WaterTankStrategy
{
    public class WaterTankTakeStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not WaterTank waterTank) return false;
            if (waterTank.State is not WaterTankState.ReadyToCollect || player.inventory.hasWater) return false;
            player.animator.PlayAnimation(PlayerAnimationState.PlayerWateringReverse);
            waterTank.ChangeTile();
            player.inventory.FillWater();
            waterTank.State = WaterTankState.Empty;
            return true;
        }
    }
}