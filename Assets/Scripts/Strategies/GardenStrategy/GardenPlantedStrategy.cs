using Enum;
using PointerObjects;

namespace Strategies.GardenStrategy
{
    public class GardenPlantedStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Garden garden) return false;
            if (garden.State is not GardenState.Planted || !player.Inventory.hasWater) return false;
            player.Inventory.UseWater();
            player.Animator.PlayAnimation(PlayerAnimationState.PlayerWatering);
            garden.StartCoroutine(garden.WaterAndGrow());
            return true;
        }
    }
}