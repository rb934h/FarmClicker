using Enum;
using PointerObjects;

namespace Strategies.GardenStrategy
{
    public class GardenSeedStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Garden garden) return false;
            if (garden.State is not GardenState.Empty || player.Inventory.currentSeed is null) return false;
            garden.StartCoroutine(garden.PlantSeed(player.Inventory.currentSeed));
            return true;
        }
    }
}