using System;
using Enum;
using PointerObjects;

namespace Strategies.GardenStrategy
{
    public class GardenSeedStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player.Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Garden garden) return false;
            if (garden._state is not GardenState.Empty || !garden.canPlantSeed) return false;
            garden.StartCoroutine(garden.PlantSeed());
            OnComplete?.Invoke();
            return true;
        }
    }
}
