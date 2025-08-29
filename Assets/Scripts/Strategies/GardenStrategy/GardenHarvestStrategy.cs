using Enum;
using PointerObjects;

namespace Strategies.GardenStrategy
{
    public class GardenHarvestStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Garden garden) return false;
            if (garden.State is not GardenState.ReadyToHarvest || !player.Inventory.CanAddItem) return false;
            player.Inventory.AddHarvestObject(garden.GetHarvestObject());
            player.Animator.PlayAnimation(PlayerAnimationState.PlayerWeeding);
            garden.Harvest();
            return true;
        }
    }
}
