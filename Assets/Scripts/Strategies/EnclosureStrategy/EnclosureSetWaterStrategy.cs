using DG.Tweening;
using Enum;
using PointerObjects;

namespace Strategies.EnclosureStrategy
{
    public class EnclosureSetWaterStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Enclosure enclosure) return false;
            if (enclosure.HasWater || !player.inventory.hasWater) return false;
            player.inventory.UseWater();
            player.animator.PlayAnimation(PlayerAnimationState.PlayerWatering);
            DOVirtual.DelayedCall(enclosure.WorkTime, enclosure.SetWaterToBowl);
            return true;
        }
    }
}