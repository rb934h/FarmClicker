using Enum;
using PointerObjects;

namespace Strategies.GardenStrategy
{
    public class GardenSeedStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Garden garden) return false;
            if (garden.State is not GardenState.Empty || !garden.canPlantSeed) return false;
            garden.StartCoroutine(garden.PlantSeed());
            return true;
        }
    }
}