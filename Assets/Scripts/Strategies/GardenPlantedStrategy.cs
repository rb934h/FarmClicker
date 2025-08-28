using Enum;

namespace Strategies
{
    public class GardenPlantedStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Garden garden) return false;
            if (garden.State is not GardenState.Planted || !player.Inventory.hasWater) return false;
            player.Inventory.UseWater();
            garden.StartCoroutine(garden.WaterAndGrow());
            return true;
        }
    }
}